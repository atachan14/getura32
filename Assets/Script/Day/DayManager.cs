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

    void Start()
    {
        if (IsHost)
        {
            
            StartTurn();
        }
    }

    public void StartTurn()
    {
        roomSizeTMP.text = RoomSetting.CI.RoomSize.ToString();
        aliveSizeTMP.text = CountAlive().ToString();
        timeTMP.text = RoomSetting.CI.TimeSize.ToString();
        remainingTime = RoomSetting.CI.TimeSize;

        SetupRoomInfoClientRpc(timeTMP.text, roomSizeTMP.text, aliveSizeTMP.text);
        StartCoroutine(TimerCoroutine());
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

        //��MatchingSet�g����
        //��MatchingSet����

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
        WaitForSeconds w = new(2f);
        foreach ((ulong p0, ulong p1, int tribute) t in LastDayData.C.PairIdList)
        {
            Vector3 direction = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f).normalized;
            GameObject p0 = NetworkManager.Singleton.ConnectedClients[t.p0].PlayerObject.gameObject;
            GameObject p1 = NetworkManager.Singleton.ConnectedClients[t.p1].PlayerObject.gameObject;

            TimeUpLeave p0tul = p0.GetComponent<TimeUpLeave>();
            TimeUpLeave p1tul = p1.GetComponent<TimeUpLeave>();
            p0tul.DayPos = (p0.transform.position);
            p1tul.DayPos = (p1.transform.position);
            p0tul.OnP0Leave(direction);
            p1tul.OnP1Leave(p0);

            yield return w;
        }
        yield return w;
        StopLeave();
        LeaverGoNightClientRpc();
    }

    void StopLeave()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClients) 
        {
            client.Value.PlayerObject.gameObject.GetComponent<TimeUpLeave>().StopLeave(); 
        }
    }

    

    [ClientRpc]
    void LeaverGoNightClientRpc()
    {
        DebuLog.C.AddDlList($"LeaverGoNightClientRpc");
        if (NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.GetComponent<MatchingStatus>().PartnerId!=null)
        {
            NightStager.C.NightStart();
        }
        else
        {
            DebuLog.C.AddDlList($"LeaverGoNightClientRpc else");
            AloneManager.C.AloneStart();
        }
    }


}
