using Unity.Netcode;
using UnityEngine;

public class PlzEffect : NetworkBehaviour
{
    public static PlzEffect C;

    [SerializeField] GameObject plzAura;

    private void Awake()
    {
        if (IsOwner) C = this;
    }


    [ServerRpc]
    public void ChangeIsPlzServerRpc(bool b)
    {
        ChangeIsPlzClientRpc(b);
    }
    [ClientRpc]
    void ChangeIsPlzClientRpc(bool b)
    {
        plzAura.SetActive(b);
    }
}