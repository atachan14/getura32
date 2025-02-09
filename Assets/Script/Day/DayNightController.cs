using Unity.Netcode.Components;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEditor.PackageManager;

public class DayNightController : NetworkBehaviour
{
    public static DayNightController C;
    public int NowNightCount { get; set; } = 0;
    bool isNight = false;

    private void Awake()
    {
        C = this;
    }

    public void GoToFlow()
    {
        CameraController.C.NightCamera();
        GoToSetAcrives();
        isNight = true;
        GoToReportServerRpc();
    }
    void GoToSetAcrives()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            client.Value.PlayerObject.gameObject.SetActive(false);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void GoToReportServerRpc()
    {
        NowNightCount++;
        DebuLog.C.AddDlList($"GoToReportSRpc NowNightCount:{NowNightCount}");
    }




    [ClientRpc]
    public void ComeBackFlowClientRpc()
    {
        if (!isNight) return;
        CameraController.C.DayCamera();
        ClientComeBackSetActives();
        ClientResetLastDayUIs();
        ComeBackReportServerRpc();
        //StartCoroutine(WaitToComeBackReportSRpc());

    }
    void ClientComeBackSetActives()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            GameObject turaa = client.Value.PlayerObject.gameObject;
            turaa.SetActive(true);
            turaa.GetComponent<NetworkTransform>().enabled = true;
            turaa.GetComponent<Rigidbody2D>().simulated = true;
            turaa.GetComponent<SpriteController>().ChangeSPRs_TMPs_A(1f);
            turaa.GetComponent<MatchingStatus>().Reset();
        }
    }

    void ClientResetLastDayUIs()
    {
        TargetInfoManager.C.Reset();
    }


    [ServerRpc(RequireOwnership = false)]
    public void ComeBackReportServerRpc()
    {
        NowNightCount--;
        Debug.Log($"ComeBackReportSRpc Id:{NetworkManager.Singleton.LocalClientId}, NowNightCount:{NowNightCount} ");
       
        if (NowNightCount == 0)
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
       
        ServerComeBackDayPos();
        StartCoroutine(EndDelay());
       
    }

   

    void ServerComeBackDayPos()
    {
        DebuLog.C.AddDlList($"start SeverComeBackDayPos:{DayAlonerManager.C.AlonerIds.Count}");
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            client.Value.PlayerObject.gameObject.GetComponent<TimeUpLeave>().ComeBackToDayFlow();
        }
    }

    IEnumerator EndDelay()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine (DaySetupper.C.ServerNewDayFlow());
    }
}
