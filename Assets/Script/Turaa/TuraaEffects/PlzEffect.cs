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
    SoManager soManager;

    private void Awake()
    {
        if (IsOwner) C = this;
    }

    private void Update()
    {
      
    }

    public void ExePlz(bool b)
    {
        plzAura.SetActive(b);
        exe = b;

        if (soManager == null) soManager = GetComponentInParent<SoManager>();
        soManager.SoSelect();
    }
}