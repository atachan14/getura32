using Unity.Netcode;
using UnityEngine;

public class DDOLReceiver : NetworkBehaviour
{
    public static DDOLReceiver C;
    ulong myId;

    private void Awake()
    {
        C = this;
    }

    private void Start()
    {
        
        myId = NetworkManager.Singleton.LocalClientId;
    }

    public void ReportCharm(int charm)
    {
        ReportCharmServerRpc(myId,charm);
    }


    [ServerRpc(RequireOwnership = false)]
    public void ReportCharmServerRpc(ulong senderId, int charm)
    {
        Debug.Log($"myId:{NetworkManager.Singleton.LocalClientId}, senderId:{senderId}, charm:{charm}");
        LPmapManager.S.ReportCharm(senderId, charm);
    }
}
