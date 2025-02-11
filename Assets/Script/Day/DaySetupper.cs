using System.Runtime.CompilerServices;
using Unity.Netcode.Components;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class DaySetupper : NetworkBehaviour
{
    public static DaySetupper C;
    ulong nowAlonerId;
    Dictionary<int, ulong> feelDictionary = new();

    private void Awake()
    {
        C = this;
    }

    public IEnumerator ServerNewDayFlow()
    {
        //yield return new WaitForSeconds(3);
        //yield return StartCoroutine(ServerDierSelect());
        Debug.Log("ServerNewDayFlow");
        yield return new WaitForSeconds(1);
        if (LastDayData.C.AlonerList.Count == 0) { ServerSeachFeelMinner(); LastDayData.C.AlonerList.Clear(); }
        else DayManager.S.StartTurn();


    }

    void ServerNewDayFlow2()
    {
        ServerFeelMinnerDie();
        MatchingSet.C.gameObject.SetActive(true);
        if(IsHost)DayManager.S.DayStartClientRpc();
    }

    void ServerSeachFeelMinner()
    {

        ReportFeelClientRpc();

    }

    [ClientRpc]
    void ReportFeelClientRpc()
    {
        if (!MatchingStatus.C.IsAlive) return;
        FeelReportServerRpc(Feel.C.Value, NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc]
    void FeelReportServerRpc(int feel, ulong id)
    {
        feelDictionary[feel] = id;
        if (feelDictionary.Count == LastDayData.C.PairIdList.Count * 2)
        {
            ServerNewDayFlow2();
        }
    }

    void ServerFeelMinnerDie()
    {
        ulong minValue = feelDictionary[feelDictionary.Keys.Min()];
        MinnerDieClientRpc(minValue);
    }

    [ClientRpc]
    void MinnerDieClientRpc(ulong minnerId)
    {
        NetworkManager.Singleton.ConnectedClients[minnerId].PlayerObject.GetComponent<DieStager>().DieStaging();
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
        WaitForSeconds w = new(4f);
        foreach (ulong id in LastDayData.C.AlonerList)
        {
            Debug.Log($"serverAlonerDie id:{id}");
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

        nowAlonerId = alonerId;
        Debug.Log($"nowAlonerId:{nowAlonerId}");
        DieStager nowAlonerDS = NetworkManager.Singleton.ConnectedClients[alonerId].PlayerObject.gameObject.GetComponent<DieStager>();

        yield return StartCoroutine(CameraController.C.ForcusAloner(nowAlonerId));
        yield return StartCoroutine(nowAlonerDS.DieStaging());
    }






}
