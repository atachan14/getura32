using Unity.Netcode.Components;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DayNightController : NetworkBehaviour
{
    public static DayNightController C;
    [SerializeField] Camera dayCamera;
    [SerializeField] Camera nightCamera;
    public List<ulong> isNowNighters { get; set; } = new();
    bool isNight = false;
    ulong myId;
    GameObject myTuraa;
    int cbReportCount = 0;

    private void Awake()
    {
        C = this;
    }

    private void Start()
    {
        myId = NetworkManager.Singleton.LocalClientId;
        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        OnNightCamera(false);
    }

    void OnNightCamera(bool b)
    {
        nightCamera.enabled = b;
        isNight = b;
        dayCamera.enabled = !b;
        myTuraa.GetComponent<MatchingStatus>().IsNight = b;
    }

    public void GoToFlow()
    {
        OnNightCamera(true);
        GoToSetAcrives();

        GoToReportServerRpc(myId);
    }
    void GoToSetAcrives()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            client.Value.PlayerObject.gameObject.SetActive(false);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void GoToReportServerRpc(ulong id)
    {
        isNowNighters.Add(id);
    }




    [ClientRpc]
    public void ComeBackFlowClientRpc()
    {
        if (!isNight) return;
        OnNightCamera(false);
        ClientComeBackSetActives();
        ServerComeBackDayPos();
        ComeBackReportServerRpc(myId);

        //StartCoroutine(WaitToComeBackReportSRpc());

    }
    void ServerComeBackDayPos()
    {
        DebuLog.C.AddDlList($"start SeverComeBackDayPos:{DayAlonerManager.C.AlonerIds.Count}");
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            if (isNowNighters.Contains(client.Value.ClientId))
            {
                GameObject turaa = client.Value.PlayerObject.gameObject;
                turaa.GetComponent<TimeUpLeave>().ComeBackToDayFlow();
            }
        }
    }
    void ClientComeBackSetActives()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            GameObject turaa = client.Value.PlayerObject.gameObject;
            turaa.SetActive(true);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ComeBackReportServerRpc(ulong id)
    {
        cbReportCount++;
        if (cbReportCount == isNowNighters.Count)
        {
            Debug.Log($"ComeBackReportSRpc  true");
            cbReportCount = 0;
            ServerFullReportAfterClientRpc();
        }
        else
        {
            Debug.Log("return");
            return;
        }
    }



    [ClientRpc]
    void ServerFullReportAfterClientRpc()
    {

        ClientComeBackSetActives2();
        ClientResetLastDayUIs();
        ComeBackReport2ServerRpc(myId);
    }

    void ClientComeBackSetActives2()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            GameObject turaa = client.Value.PlayerObject.gameObject;

            if (turaa.GetComponent<MatchingStatus>().IsAlive) continue;

            turaa.GetComponent<NetworkTransform>().enabled = true;
            turaa.GetComponent<Rigidbody2D>().simulated = true;
            turaa.GetComponent<SpriteController>().ChangeSPRs_TMPs_A(1f);
        }
    }
    void ClientResetLastDayUIs()
    {
        NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<MatchingStatus>().Reset();

    }


    [ServerRpc(RequireOwnership = false)]
    public void ComeBackReport2ServerRpc(ulong id)
    {
        isNowNighters.Remove(id);
        Debug.Log($"isNowNighters.Count:{isNowNighters.Count}");
        if (isNowNighters.Count == 0)
        {
            Debug.Log($"ComeBackReportSRpc  true");
            ServerFullReportAfter();
        }
        else
        {
            Debug.Log("return");
            return;
        }

    }

    void ServerFullReportAfter()
    {


        StartCoroutine(EndDelay());

    }





    IEnumerator EndDelay()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(DaySetupper.C.ServerNewDayFlow());
    }
}
