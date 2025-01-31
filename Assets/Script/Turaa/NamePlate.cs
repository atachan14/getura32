using UnityEngine;
using TMPro;
using Unity.Netcode;

using Unity.Collections;

public class NamePlate : NetworkBehaviour
{
    public NetworkVariable<FixedString64Bytes> TuraaName { get; set; } = new NetworkVariable<FixedString64Bytes>();
    [SerializeField] private TextMeshProUGUI nameTMP;

    void Start()
    {
        if (IsOwner)
        {
            SetTuraaNameServerRpc(PlayerPrefs.GetString("TuraaName"));
        }
    }
    void Update()
    {
        nameTMP.text = TuraaName.Value.ToString();
    }
    [ServerRpc]
    void SetTuraaNameServerRpc(string newName)
    {
        Debug.Log("namePlate SetTuraaNameServerRpc");
        TuraaName.Value = newName;
        nameTMP.text = newName;
    }
    public void SetTMP(string newName)
    {
        nameTMP.text = newName;
    }

    public string GetName()
    {
        return nameTMP.text;
    }




}
