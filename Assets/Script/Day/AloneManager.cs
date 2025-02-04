using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AloneManager : NetworkBehaviour
{
    public static AloneManager C;
    public List<ulong> AlonerIds { get; set; } = new();
    void Start()
    {
        C = this;
    }

    void Update()
    {

    }

    public void ClientAloneStart()
    {
        DebuLog.C.AddDlList("alone start");
        ReportAlonerServerRpc(NetworkManager.Singleton.LocalClientId);
        DebuLog.C.AddDlList("after ReportAlonerServerRpc");
    }
    [ServerRpc(RequireOwnership = false)]
    public void ReportAlonerServerRpc(ulong id)
    {
        DebuLog.C.AddDlList($"ReportAlonerServerRpc id:{id}");
        AlonerIds.Add(id);
    }
}
