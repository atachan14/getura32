using System.Runtime.CompilerServices;
using Unity.Netcode.Components;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DaySetupper : NetworkBehaviour
{
    public static DaySetupper C;
    bool dieStaging = false;
    ulong nowAlonerId;

    private void Awake()
    {
        C = this;
    }

    public void ServerNewDayFlow()
    {
        ServerComeBackDayPos();
        ServerAlonersDie();
    }
    void ServerComeBackDayPos()
    {
        DebuLog.C.AddDlList($"start SeverComeBackDayPos:{AloneManager.C.AlonerIds.Count}");
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            client.Value.PlayerObject.gameObject.GetComponent<TimeUpLeave>().ComeBackToDayFlow();
        }
    }

    void ServerAlonersDie()
    {
        DebuLog.C.AddDlList($"start SeverAlonersDie AlonerIds.Count:{AloneManager.C.AlonerIds.Count}");
        while (AloneManager.C.AlonerIds.Count != 0)
        {
            if (dieStaging) continue;
            dieStaging = true;
            ulong alonerId = AloneManager.C.AlonerIds[0];
            AlonerDieClientRpc(alonerId);

        }
        DebuLog.C.AddDlList("ServerAlonersDie end");
    }

    [ClientRpc]
    void AlonerDieClientRpc(ulong aloner)
    {
        nowAlonerId = aloner;
        DebuLog.C.AddDlList($"nowAlonerId:{nowAlonerId}");

        CameraController.C.ForcusAloner(nowAlonerId);
        StartCoroutine(ClientAlonerDie());
    }

    IEnumerator ClientAlonerDie()
    {
        WaitForSeconds w = new WaitForSeconds(2f);
        DebuLog.C.AddDlList("w2f");
        yield return w;
        NetworkManager.Singleton.ConnectedClients[nowAlonerId].PlayerObject
            .gameObject.GetComponent<DieStager>().DieStaging();
        DebuLog.C.AddDlList("w2f2");
        yield return w;

        if (IsHost)
        {
            AloneManager.C.AlonerIds.RemoveAt(0);
            dieStaging = false;
        }
    }



}
