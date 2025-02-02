using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;

public class DaySetuper : NetworkBehaviour
{
    public static DaySetuper S;
    void Awake()
    {
        S = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DaySetupStart()
    {
        if (!IsHost) DebuLog.C.AddDlList("ERROR DaySetupStart !IsHost");

        InDayClientRpc();
        ComeBackDayPos();

    }

    void ComeBackDayPos()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            client.Value.PlayerObject.gameObject.GetComponent<TimeUpLeave>().StartSetupComeBack();
        }
    }

    [ClientRpc]
    void InDayClientRpc()
    {
        CameraController.C.DayCamera();
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            GameObject turaa = client.Value.PlayerObject.gameObject;
            turaa.SetActive(true);
            turaa.GetComponent<NetworkTransform>().enabled = true;
            turaa.GetComponent<Rigidbody2D>().simulated = true;
        }
    }
}
