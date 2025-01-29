using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

public class PartnerManager : NetworkBehaviour
{
    public static PartnerManager S;
    public static PartnerManager C;
    List<(ulong p0, ulong p1)> pairIdList = new();

    ulong myId;
    ulong? partnerId;
    GameObject myTuraa;
    public GameObject PartnerTuraa { get; set; }
    void Awake()
    {
        if (IsHost) S = this;
        C = this;
        myId = NetworkManager.Singleton.LocalClientId;
        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
    }

    [ServerRpc]
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

    [ServerRpc]
    public void SplitPartnerServerRpc(ulong splitId) 
    {
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
        foreach ((ulong p0,ulong p1) pairId in pairIdList )
        {
            updatePartnerClientRpc(pairId.p0, pairId.p1);
        }
        UpdateMatchingStatus();
    }

    [ClientRpc]
    public void ResetPartnerClientRpc()
    {
        partnerId = null;
        PartnerTuraa = null;
    }

    [ClientRpc]
    public void updatePartnerClientRpc(ulong p0, ulong p1)
    {
        if (myId == p0) partnerId = p1;
        if (myId == p1) partnerId = p0;
        PartnerTuraa = NetworkManager.Singleton.ConnectedClients[(ulong)partnerId].PlayerObject.gameObject;
    }

    void UpdateMatchingStatus()
    {
        myTuraa.GetComponent<MatchingStatus>().PartnerTuraa = PartnerTuraa;
        myTuraa.GetComponent<MatchingStatus>().PartnerId = partnerId;
    }
}
