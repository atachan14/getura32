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
    bool dieStaging;

    private void Awake()
    {
        C = this;
    }

    public IEnumerator ServerNewDayFlow()
    {
        
        yield return StartCoroutine(ServerDierSelect());


        DayManager.S.StartTurn();





    }

   



    IEnumerator ServerDierSelect()
    {
        if (LastDayData.C.AlonerList.Count == 0)
        {
            Debug.Log("severDierSelect true");
            yield return StartCoroutine(ServerFeelMinDie());
        }
        else
        {
            Debug.Log("severDierSelect else");
            yield return StartCoroutine(ServerAlonerDie());
        }
    }

    IEnumerator ServerFeelMinDie()
    {
        Debug.Log("ServerFeelMinDie");
        yield return null;
    }

    IEnumerator ServerAlonerDie()
    {
        Debug.Log($"{LastDayData.C.AlonerList}");
        WaitForSeconds w = new WaitForSeconds(4f);
        foreach (ulong id in LastDayData.C.AlonerList)
        {
            AlonerDieClientRpc(id);
            yield return w;
        }
        LastDayData.C.AlonerList.Clear();
        Debug.Log("ServerAlonersDie end");
    }

    [ClientRpc]
    void AlonerDieClientRpc(ulong alonerId)
    {
        StartCoroutine(AlonerDieCoroutine(alonerId));
    }

    IEnumerator AlonerDieCoroutine(ulong alonerId)
    {
        Debug.Log($"nowAlonerId:{nowAlonerId}");
        nowAlonerId = alonerId;
        DieStager nowAlonerDS = NetworkManager.Singleton.ConnectedClients[alonerId].PlayerObject.gameObject.GetComponent<DieStager>();
        Debug.Log($"after ForcusAloner");
        yield return StartCoroutine(CameraController.C.ForcusAloner(nowAlonerId));
        yield return StartCoroutine(nowAlonerDS.DieStaging());
    }






}
