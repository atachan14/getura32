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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        TestServerRpc(NetworkManager.Singleton.LocalClientId);
        CameraController.C.DayCamera();
        DebuLog.C.AddDlList("befor ClientComeBackSetActive");
        ClientComeBackSetActives(); DebuLog.C.AddDlList("befor ComeBackReportServerRpc");
        ComeBackReportServerRpc(); DebuLog.C.AddDlList("after ComeBackReportServerRpc");
        //StartCoroutine(WaitToComeBackReportSRpc());
       
    }

    [ServerRpc(RequireOwnership = false)]
    public void TestServerRpc(ulong id)
    {
        DebuLog.C.AddDlList($"test : {id}");
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
        DebuLog.C.AddDlList($"ComeBackReportSRpc NowNightCount:{NowNightCount} ");
        NowNightCount--;

        if (NowNightCount == 0)
        {
            DebuLog.C.AddDlList($"ComeBackReportSRpc  true");
            ServerFullReportAfter();
        }
    }

    void ServerFullReportAfter()
    {
        DaySetupper.C.ServerNewDayFlow();
    }

}
