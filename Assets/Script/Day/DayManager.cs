using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    bool isNight = false;

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
        StartCoroutine(TuraasLeaveColutin());


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

    IEnumerator TuraasLeaveColutin()
    {
        WaitForSeconds w = new WaitForSeconds(2f);
        foreach ((ulong p0, ulong p1, int tribute) t in LastDayData.C.PairIdList)
        {
            Vector3 direction = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f).normalized;
            GameObject p0 = NetworkManager.Singleton.ConnectedClients[t.p0].PlayerObject.gameObject;
            GameObject p1 = NetworkManager.Singleton.ConnectedClients[t.p1].PlayerObject.gameObject;

            LastDayData.C.TuraaPosDict[t.p0] = (p0.transform.position);
            LastDayData.C.TuraaPosDict[t.p1] = (p1.transform.position);

            p0.GetComponent<TimeUpLeave>().OnP0Leave(direction);
            p0.GetComponent<TimeUpLeave>().OnP1Leave(p0);

            yield return w;
        }
        yield return new WaitForSeconds(3f);
        StopLeave();
        PairGoNight();
    }

    void StopLeave()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClients) 
        {
            client.Value.PlayerObject.gameObject.GetComponent<TimeUpLeave>().StopLeave(); 
        }
    }

    void PairGoNight()
    {
        foreach (ulong id in LastDayData.C.TuraaPosDict.Keys) { PairGoNightClientRpc(id); }
        if (!isNight) DebuLog.C.AddDlList("DyningAction");
    }

    [ClientRpc]
    void PairGoNightClientRpc(ulong id)
    {
        DebuLog.C.AddDlList($"PairGoNightClientRpc{id},{NetworkManager.Singleton.LocalClientId}");
        if (id == NetworkManager.Singleton.LocalClientId)
        {
            NightManager.C.NightStart();
            isNight = true;
        }
    }


}
