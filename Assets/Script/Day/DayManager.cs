using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class DayManager : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI roomSizeTMP;
    [SerializeField] TextMeshProUGUI aliveSizeTMP;
    [SerializeField] TextMeshProUGUI timeTMP;
    private int remainingTime;

    [SerializeField] GameObject otherThenCamera;

    void Start()
    {
        if (IsHost)
        {
            roomSizeTMP.text = RoomSetting.CI.RoomSize.ToString();
            aliveSizeTMP.text = CountAlive().ToString();
            timeTMP.text = RoomSetting.CI.TimeSize.ToString();
            remainingTime = RoomSetting.CI.TimeSize;

            SetupRoomInfoClientRpc(timeTMP.text, roomSizeTMP.text, aliveSizeTMP.text);
            StartCoroutine(TimerCoroutine());
        }
    }
    int CountAlive()
    {
        if (!IsHost) DebuLog.C.AddDlList("!IsHost! CountAlive ");
        int aliveCount = 0;
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            GameObject turaa = client.PlayerObject.GameObject();

            if (turaa != null && turaa.GetComponent<OwnerPlayer>().IsAlive)
            {
                aliveCount++;
            }
        }
        return aliveCount;
    }
    [ClientRpc]
    public void SetupRoomInfoClientRpc(string t, string rs, string aliveSize)
    {
        timeTMP.text = t;
        roomSizeTMP.text = rs;
        aliveSizeTMP.text = aliveSize;
    }


    private System.Collections.IEnumerator TimerCoroutine()
    {
        if (!IsHost) DebuLog.C.AddDlList("!IsHost! TimerCoroutine ");
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;
            timeTMP.text = remainingTime.ToString();
            UpdateTimeClientRpc(timeTMP.text);
        }
        AfterTimeUpFlow();
    }

    [ClientRpc]
    public void UpdateTimeClientRpc(string t)
    {
        timeTMP.text = t;
    }

    public void AfterTimeUpFlow()
    {
        if (!IsHost) DebuLog.C.AddDlList("!IsHost! AfterTimeUpFlow ");
        SavePairIdList();

        //Å™MatchingSetégÇ¶ÇÈ
        //Å´MatchingSetè¡Ç∑

        StopForTimeUpClientRpc();
        StartCoroutine(TuraaLeave());


    }


    void SavePairIdList()
    {
        LastDayData.C.PairIdList = PartnerManager.C.PairIdList;
        DebuLog.C.AddDlList($"SavePairIdList[{string.Join(", ", LastDayData.C.PairIdList)}]");
    }

    [ClientRpc]
    void StopForTimeUpClientRpc()
    {
        DebuLog.C.AddDlList("timeup");
        otherThenCamera.SetActive(false);
    }

    IEnumerator TuraaLeave()
    {
        WaitForSeconds w = new WaitForSeconds(2f);
        foreach ((ulong p0, ulong p1, int tribute) t in LastDayData.C.PairIdList)
        {
            Vector3 direction = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f).normalized;
            GameObject p0 = NetworkManager.Singleton.ConnectedClients[t.p0].PlayerObject.gameObject;
            p0.GetComponent<TimeUpLeave>().OnP0Leave(direction);

            GameObject p1 = NetworkManager.Singleton.ConnectedClients[t.p1].PlayerObject.gameObject;
            p0.GetComponent<TimeUpLeave>().OnP1Leave(p0);

            yield return w;
        }
        yield return new WaitForSeconds(5f);
        SLD.SingleLoad(SNM.Night);
    }


}
