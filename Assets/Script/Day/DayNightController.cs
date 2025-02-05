using Unity.Netcode.Components;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DayNightController : NetworkBehaviour
{
    public static DayNightController C;
    public int NowNightCount { get; set; } = 0;

    private void Awake()
    {
        C = this;
    }

    public void GoToFlow()
    {
        CameraController.C.NightCamera();
        GoToSetAcrives();
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
    public void ClientComeBackFlow()
    {
        DebuLog.C.AddDlList("start ClientComeBackFlow");
        CameraController.C.DayCamera();
        DebuLog.C.AddDlList("befor ClientComeBackSetActive");
        ClientComeBackSetActives(); DebuLog.C.AddDlList("befor ComeBackReportServerRpc");
        ComeBackReportServerRpc(); DebuLog.C.AddDlList("after ComeBackReportServerRpc");
        //StartCoroutine(WaitToComeBackReportSRpc());

    }

    [ServerRpc(RequireOwnership = false)]
    public void TestServerRpc(ulong id)
    {
        DebuLog.C.AddDlList($"test DayNight : {id}");
    }

    //IEnumerator WaitToComeBackReportSRpc()
    //{
    //    yield return new WaitForSeconds(3f);

    //}


    void ClientComeBackSetActives()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            GameObject turaa = client.Value.PlayerObject.gameObject;
            turaa.SetActive(true);
            turaa.GetComponent<NetworkTransform>().enabled = true;
            turaa.GetComponent<Rigidbody2D>().simulated = true;
            turaa.GetComponent<SpriteController>().ChangeSPRs_A(1f);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ComeBackReportServerRpc()
    {
        NowNightCount--;
        DebuLog.C.AddDlList($"ComeBackReportSRpc Id:{NetworkManager.Singleton.LocalClientId}, NowNightCount:{NowNightCount} ");
       
        //SeverRpc送れないバグ
        if (NowNightCount == 0)
        {
            DebuLog.C.AddDlList($"ComeBackReportSRpc  true");
            ServerFullReportAfter();
        }
        else
        {
            DebuLog.C.AddDlList("return");
            return;
        }

        //StartCoroutine(TempServerFullReportAfter());
        //ServerFullReportAfter();
    }

    void ServerFullReportAfter()
    {
        DaySetupper.C.ServerNewDayFlow();
    }

    IEnumerator TempServerFullReportAfter()
    {
        DebuLog.C.AddDlList("TempServerFull");
        yield return new WaitForSeconds(3f);
        DebuLog.C.AddDlList("TempServerFullReportAfter");
        DaySetupper.C.ServerNewDayFlow();
    }
}
