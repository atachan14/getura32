using UnityEngine;
using TMPro;
using Unity.Netcode;

using Unity.Collections;

public class NamePlate : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI nameTMP;
    [SerializeField] TextMeshProUGUI lpTMP;
    public NetworkVariable<FixedString64Bytes> TuraaName { get; set; } = new NetworkVariable<FixedString64Bytes>();
    
    private int displayLp;
    public int DisplayLp
    {
        get { return displayLp; }
        set
        {
            displayLp = value;
            lpTMP.text = value.ToString();
            float t = Mathf.InverseLerp(0, 100, displayLp);
            lpTMP.color = Color.Lerp(Color.blue, Color.magenta, t);
        }
    }

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
    }
    

    public string GetName()
    {
        return nameTMP.text;
    }




}
