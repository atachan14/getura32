using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DayAlonerManager : NetworkBehaviour
{
    public static DayAlonerManager C;
    public List<ulong> AlonerIds { get; set; } = new();
    private void Awake()
    {
        C = this;
    }

    public void ClientAloneStart()
    {
        ReportAlonerServerRpc(NetworkManager.Singleton.LocalClientId);
        Feel.C.AloneFeel();
        DebuLog.C.AddDlList("after ReportAlonerServerRpc");
    }
    [ServerRpc(RequireOwnership = false)]
    public void ReportAlonerServerRpc(ulong id)
    {
        LastDayData.C.AlonerList.Add(id);
        DebuLog.C.AddDlList($"ReportAlonerServerRpc id:{id} , ListCount:{AlonerIds.Count}");
    }
}
