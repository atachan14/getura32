using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NightCalcer : NetworkBehaviour
{
    public static NightCalcer C;
    ulong myId;
    List<(ulong sId,ulong tId)> serverList=new();

    void Awake()
    {
        C = this;
    }

    private void Start()
    {
        myId = NetworkManager.Singleton.LocalClientId;
    }

    public void StartCalc()
    {
        NamePlate partnerNamePlate = MatchingStatus.C.PartnerTuraa.GetComponent<NamePlate>();
        int partnerLp = partnerNamePlate.DisplayLp;

        Gold.C.Value += PartnerManager.C.tribute;
        Feel.C.Value += partnerLp - 50;
        Charm.C.Value += partnerLp / 10;
        ReportPartnerServerRpc(myId, MatchingStatus.C.PartnerId);
       
    }

  

    [ServerRpc(RequireOwnership =false)]
    void ReportPartnerServerRpc(ulong senderId,ulong partnerId)
    {
        serverList.Add((senderId, partnerId));

        if (serverList.Count != DayNightController.C.NowNightCount) return;

        foreach (var item in serverList)
        {
            LPmapManager.S.NightReportReceive(item.sId, item.tId);
        }
        ServerDeliverLP();
        StartCoroutine(NightCalcEndCoroutin());
    }

    void ServerDeliverLP()
    {
        int rs = RoomSetting.CI.RoomSize;
        for (int i = 0; i < rs; i++)
        {
            for (int j = 0; j < rs; j++)
            {
                if (i == j) continue;
                int lp = LPmapManager.S.GetLpFromMeTage(i, j);
                DeliverLPClientRpc(i, j, lp);
            }
        }
        DebuLog.C.AddDlList("DeliverLP end");
    }

    [ClientRpc]
    void DeliverLPClientRpc(int me, int target, int lp)
    {
        int myIntId = (int)NetworkManager.Singleton.LocalClientId;
        if (myIntId != me) return;
        if (!NetworkManager.Singleton.ConnectedClients.ContainsKey((ulong)target)) DebuLog.C.AddDlList($"DliverLPCRpc:???tage unknown???");

        GameObject tage = NetworkManager.Singleton.ConnectedClients[(ulong)target].PlayerObject.gameObject;
        tage.GetComponent<NamePlate>().DisplayLp = lp;
    }


    IEnumerator NightCalcEndCoroutin()
    {
        yield return new WaitForSeconds(2);
        DayNightController.C.ComeBackFlowClientRpc();
    }
   
}
