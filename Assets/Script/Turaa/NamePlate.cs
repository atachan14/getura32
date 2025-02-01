using UnityEngine;
using TMPro;
using Unity.Netcode;

using Unity.Collections;
using System.Runtime.CompilerServices;

public class NamePlate : NetworkBehaviour
{
    [SerializeField] MatchingStatus mStatus;
    public NetworkVariable<FixedString64Bytes> TuraaName { get; set; } = new NetworkVariable<FixedString64Bytes>();
    [SerializeField] public TextMeshProUGUI nameTMP;
    [SerializeField] TextMeshProUGUI lpTMP;
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
            Debug.Log("ddd");

            //DebuLog.C.AddDlList("OnNetworkSpawn IsOwner");
            SetTuraaNameServerRpc(PlayerPrefs.GetString("TuraaName"));
        }
        if (IsClient)
        {
            OnTuraaNameChanged(default, TuraaName.Value);
        }
        TuraaName.OnValueChanged += OnTuraaNameChanged;
    }
    
    void SetupNP()
    {


       
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
       
    }
    [ServerRpc]
    void SetTuraaNameServerRpc(string newName)
    {
        //Debug.Log("namePlate SetTuraaNameServerRpc");
        TuraaName.Value = newName;
        //nameTMP.text = newName;
    }
    private void OnTuraaNameChanged(FixedString64Bytes oldValue, FixedString64Bytes newValue)
    {
        nameTMP.text = newValue.ToString();
        Debug.Log($"mStatus.IsRed:{mStatus.IsRed}");
        Debug.Log($"nameTMP.color != :{nameTMP.color}");


        if (mStatus.IsRed) nameTMP.color = Color.red;
        else if (mStatus.IsPlz) nameTMP.color = Color.yellow;
        else if (mStatus.IsCant) nameTMP.color = Color.gray;
        else if (mStatus.IsStick) nameTMP.color = new Color(63 / 255f, 226 / 255f, 72 / 255f);
        else if (mStatus.IsPink) nameTMP.color = new Color(255 / 255f, 100 / 255f, 220 / 255f);
        else nameTMP.color = Color.white;
    }
    void Update()
    {
        //nameTMP.text = TuraaName.Value.ToString();
    }
   
   

    public string GetName()
    {
        return nameTMP.text;
    }

    public Color GetColor()
    {
        return nameTMP.color;
    }




}
