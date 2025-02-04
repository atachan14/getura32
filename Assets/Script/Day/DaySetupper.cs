using System.Runtime.CompilerServices;
using Unity.Netcode.Components;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DaySetupper : NetworkBehaviour
{
    public static DaySetupper C;
    ulong nowAlonerId;

    private void Awake()
    {
        C = this;
    }

    public void ServerNewDayFlow()
    {
        ServerComeBackDayPos();
       
        DayManager.S.StartTurn();



    }
    void ServerComeBackDayPos()
    {
        DebuLog.C.AddDlList($"start SeverComeBackDayPos:{DayAlonerManager.C.AlonerIds.Count}");
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            client.Value.PlayerObject.gameObject.GetComponent<TimeUpLeave>().ComeBackToDayFlow();
        }
    }


    //ì‰ÇÃÉoÉOÅB
    //void ServerAlonersDie()
    //{
    //DebuLog.C.AddDlList($"start SeverAlonersDie AlonerIds.Count:{DayAlonerManager.C.AlonerIds.Count}");
    //while (DayAlonerManager.C.AlonerIds.Count != 0)
    //{
    //    if (dieStaging) continue;
    //    dieStaging = true;
    //    ulong alonerId = DayAlonerManager.C.AlonerIds[0];
    //    AlonerDieClientRpc(alonerId);

    //}
    //DebuLog.C.AddDlList("ServerAlonersDie end");
    //}

    //[ClientRpc]
    //void AlonerDieClientRpc(ulong aloner)
    //{
    //    nowAlonerId = aloner;
    //    DebuLog.C.AddDlList($"nowAlonerId:{nowAlonerId}");

    //    CameraController.C.ForcusAloner(nowAlonerId);
    //    StartCoroutine(ClientAlonerDie());
    //}

    //IEnumerator ClientAlonerDie()
    //{
    //    WaitForSeconds w = new WaitForSeconds(2f);
    //    DebuLog.C.AddDlList("w2f");
    //    yield return w;
    //    NetworkManager.Singleton.ConnectedClients[nowAlonerId].PlayerObject
    //        .gameObject.GetComponent<DieStager>().DieStaging();
    //    DebuLog.C.AddDlList("w2f2");
    //    yield return w;

    //    if (IsHost)
    //    {
    //        DayAlonerManager.C.AlonerIds.RemoveAt(0);
    //        dieStaging = false;
    //    }
    //}

    


}
