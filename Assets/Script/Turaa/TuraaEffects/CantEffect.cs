using Unity.Netcode;
using UnityEngine;

public class CantEffect : NetworkBehaviour
{
    public static CantEffect C;
    [SerializeField] GameObject cantAura;

    private void Awake()
    {
        if (IsOwner) C = this;
    }

    [ServerRpc]
    public void ChangeIsCantServerRpc(bool b)
    {
        ChangeIsCantClientRpc(b);
    }
    [ClientRpc]
    void ChangeIsCantClientRpc(bool b)
    {
        cantAura.SetActive(b);
    }
}