using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;
using System.Data.SqlTypes;

public class PartnerManager : NetworkBehaviour
{
    public static PartnerManager C;
    public List<(ulong p0, ulong p1, int tribute)> PairIdList { get; set; } = new();

    ulong myId;
    ulong? partnerId;
    int tribute;
    bool isP0;
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
    public void ChangePartnerServerRpc(ulong newId0, ulong newId1, int tribute)
    {
        RemoveOldPartner(newId0, newId1);
        PairIdList.Add((newId0, newId1, tribute));
        ClientUpdate();
    }

    void RemoveOldPartner(ulong p0, ulong p1)
    {
        PairIdList.RemoveAll(p => p.p0 == p0 || p.p1 == p0 || p.p0 == p1 || p.p1 == p1);
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
        PairIdList.RemoveAll(p => p.p0 == splitId || p.p1 == splitId);
    }

    void ClientUpdate()
    {
        ResetPartnerClientRpc();
        foreach ((ulong p0, ulong p1, int tribute) pairId in PairIdList)
        {
            UpdateThisFieldClientRpc(pairId.p0, pairId.p1, pairId.tribute);
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
    public void UpdateThisFieldClientRpc(ulong p0, ulong p1, int tribute)
    {
        DebuLog.C.AddDlList("UpdatePartnerClientRpc");
        if (myId == p0) { partnerId = p1; this.isP0 = true; DebuLog.C.AddDlList("me p0"); }
        if (myId == p1) { partnerId = p0; this.isP0 = false; DebuLog.C.AddDlList("me p1"); }
        PartnerTuraa = NetworkManager.Singleton.ConnectedClients[(ulong)partnerId].PlayerObject.gameObject;
        this.tribute = tribute;

        DebuLog.C.AddDlList($"UpdatePartnerClientRpc partnerId:{partnerId}");

    }
    [ClientRpc]
    void UpdateMatchingStatusClientRpc()
    {
        DebuLog.C.AddDlList($"UpdateMatchingStatus1 mStatus==null:{mStatus == null}");
        mStatus.PartnerTuraa = PartnerTuraa;
        mStatus.PartnerId = partnerId;
        mStatus.IsP0 = this.isP0;
        if (isP0) TopInfo.tributeDict[(ulong)partnerId] = -1 * tribute;
        if (!isP0) TopInfo.tributeDict[(ulong)partnerId] =  tribute;
        TopInfo.C.ShowStickTributeTMP();
        DebuLog.C.AddDlList("UpdateMatchingStatus2");

    }
}
