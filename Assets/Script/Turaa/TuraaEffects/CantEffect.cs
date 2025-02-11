using Unity.Netcode;
using UnityEngine;

public class CantEffect : NetworkBehaviour
{
    public static CantEffect C;
    //[SerializeField] GameObject cantAura;

    private void Awake()
    {
        if (IsOwner) C = this;
    }
  
    [ServerRpc]
    public void ExeServerRpc(bool b)
    {
        ExeClientRpc(b);
    }
    [ClientRpc]
    void ExeClientRpc(bool b)
    {
        if (b)
        {
            //cantAura.SetActive(true);
            tag = "Untagged";
            SpriteRenderer[] mySPRs = GetComponentsInChildren<SpriteRenderer>(true);

            foreach (SpriteRenderer spr in mySPRs)
            {
                spr.color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
            }
        }
        else
        {
            //cantAura.SetActive(false);
            tag = "CharacterClick";
            SpriteRenderer[] mySPRs = GetComponentsInChildren<SpriteRenderer>(true);

            foreach (SpriteRenderer spr in mySPRs)
            {
                spr.color = new Color(1, 1, 1, 1f);
            }
        }
    }


}