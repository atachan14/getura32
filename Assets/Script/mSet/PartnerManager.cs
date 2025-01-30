using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

public class PartnerManager : NetworkBehaviour
{
    public static PartnerManager C;
    List<(ulong p0, ulong p1)> pairIdList = new();

    ulong myId;
    ulong? partnerId;
    GameObject myTuraa;
    MatchingStatus mStatus;
    public GameObject PartnerTuraa { get; set; }
    void Awake()
    {
        C = this;
    }
    private void Start()
    {
        myId = NetworkManager.Singleton.LocalClientId;
        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        mStatus = myTuraa.GetComponent<MatchingStatus>();
    }



    [ServerRpc(RequireOwnership = false)]
    public void ChangePartnerServerRpc(ulong newId0, ulong newId1)
    {
        RemoveOldPartner(newId0, newId1);
        pairIdList.Add((newId0, newId1));
        ClientUpdate();
    }

    void RemoveOldPartner(ulong p0, ulong p1)
    {
        pairIdList.RemoveAll(p => p.p0 == p0 || p.p1 == p0 || p.p0 == p1 || p.p1 == p1);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SplitPartnerServerRpc(ulong splitId)
    {
        DebuLog.C.AddDlList("SplitPartnerServerRpc");
        RemoveSplitPartner(splitId);
        ClientUpdate();

    }
    void RemoveSplitPartner(ulong splitId)
    {
        pairIdList.RemoveAll(p => p.p0 == splitId || p.p1 == splitId);
    }

    void ClientUpdate()
    {
        ResetPartnerClientRpc();
        foreach ((ulong p0, ulong p1) pairId in pairIdList)
        {
            UpdatePartnerClientRpc(pairId.p0, pairId.p1);
        }
        UpdateMatchingStatusClientRpc();
    }

    [ClientRpc]
    public void ResetPartnerClientRpc()
    {
        partnerId = null;
        PartnerTuraa = null;
    }

    [ClientRpc]
    public void UpdatePartnerClientRpc(ulong p0, ulong p1)
    {
        DebuLog.C.AddDlList("UpdatePartnerClientRpc");
        if (myId == p0) partnerId = p1;
        if (myId == p1) partnerId = p0;
        PartnerTuraa = NetworkManager.Singleton.ConnectedClients[(ulong)partnerId].PlayerObject.gameObject;
        DebuLog.C.AddDlList($"UpdatePartnerClientRpc partnerId:{partnerId}");

    }
    [ClientRpc]
    void UpdateMatchingStatusClientRpc()
    {
        DebuLog.C.AddDlList($"UpdateMatchingStatus1 mStatus==null:{mStatus == null}");
        mStatus.PartnerTuraa = PartnerTuraa;
        mStatus.PartnerId = partnerId;
        DebuLog.C.AddDlList("UpdateMatchingStatus2");

    }
}
