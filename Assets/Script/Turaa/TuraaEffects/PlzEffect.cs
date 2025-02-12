using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlzEffect : NetworkBehaviour
{
    public static PlzEffect C;

    [SerializeField] GameObject plzAura;
    [SerializeField] GameObject plz0;
    [SerializeField] GameObject plz1;
    [SerializeField] GameObject plz2;
    [SerializeField] GameObject plz3;
    [SerializeField] GameObject plz4;
    [SerializeField] GameObject plz5;
    [SerializeField] GameObject plz6;
    [SerializeField] GameObject plz7;
    bool exe;

    private void Awake()
    {
        if (IsOwner) C = this;
    }

    private void Update()
    {
        //if (exe)
        //{
        //    if (plz1roteR)
        //    {
        //        if (plz1rote > 300) plz1roteR = true;
        //        plz1rote += plz1roteSpeed;
        //        plz1.transform.Rotate(0, 0, plz1rote * Time.deltaTime);
        //    }
        //    else
        //    {
        //        if (plz1rote < 50) plz1roteR = false;
        //        plz1rote -= plz1roteSpeed;
        //        plz1.transform.Rotate(0, 0, plz1rote * Time.deltaTime);
        //    }
        //    if (plz1roteSpeedR)
        //    {
        //        if (plz1roteSpeed > 5) plz1roteSpeedR = true;
        //        plz1roteSpeed += 1;
        //    }
        //    else
        //    {
        //        if (plz1roteSpeed < 0.5) plz1roteSpeedR = false;
        //        plz1roteSpeed -= 1;
        //    }
        //}
    }

    [ServerRpc]
    public void ExeServerRpc(bool b)
    {
        ExeClientRpc(b);
    }
    [ClientRpc]
    void ExeClientRpc(bool b)
    {
        plzAura.SetActive(b);
        exe = b;
        //SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        //TextMeshProUGUI[] tmps = GetComponentsInChildren<TextMeshProUGUI>();

        //if (b)
        //{
        //    foreach (SpriteRenderer sr in srs)
        //    {
        //        sr.sortingOrder += 100;
        //    }
        //    foreach (TextMeshProUGUI tmp in tmps)
        //    {
        //        tmp.geometrySortingOrder += 1000;
        //    }
        //}
        //else
        //{
        //    foreach (SpriteRenderer sr in srs)
        //    {
        //        sr.sortingOrder -= 100;
        //    }
        //    foreach (TextMeshProUGUI tmp in tmps)
        //    {
        //        tmp.geometrySortingOrder -= 1000;
        //    }
        //}
    }
}