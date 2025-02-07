using Unity.Netcode;
using UnityEngine;

public class SplitManager : MonoBehaviour
{
    public static SplitManager C;
    ulong myId;
    private void Awake()
    {
        C = this;
    }

    private void Start()
    {
        myId = NetworkManager.Singleton.LocalClientId;
    }
    public void Split()
    {
        //æ‚É‘—M‚µ‚Ä‚©‚ç•Ê‚ê‚éB
        SplitServerRpc((ulong)MatchingStatus.C.PartnerId);
        MatchingStatus.C.PartnerId = null;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SplitServerRpc(ulong oldPartnerId)
    {
        SplitClientRpc(oldPartnerId);
    }
    [ClientRpc]
    void SplitClientRpc(ulong oldPartnerId)
    {
        if (myId != oldPartnerId) return;
        MatchingStatus.C.PartnerId = null;
    }
}
