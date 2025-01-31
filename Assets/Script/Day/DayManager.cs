using System.Collections.Generic;
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

        TimeUpClientRpc();


    }
  

    void SavePairIdList()
    {
        LastDayData.C.PairIdList = PartnerManager.C.PairIdList;
        DebuLog.C.AddDlList($"SavePairIdList[{string.Join(", ", LastDayData.C.PairIdList)}]");
    }

    [ClientRpc]
    void TimeUpClientRpc()
    {
        DebuLog.C.AddDlList("timeup");
        otherThenCamera.SetActive(false);
    }
}
