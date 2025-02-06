using UnityEngine;
using TMPro;
using Unity.Netcode;

using Unity.Collections;
using static UnityEngine.Rendering.DebugUI;

public class NamePlate : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI nameTMP;
    [SerializeField] TextMeshProUGUI lpTMP;
    public NetworkVariable<FixedString64Bytes> TuraaName { get; set; } = new NetworkVariable<FixedString64Bytes>();
    public Color NameColor
    {
        get => nameTMP.color;
        set => nameTMP.color = value;
    }

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
            ChangeColor();
        }
    }
    [ClientRpc]
    public void StartClientRpc()
    {
        ChangeColor();
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

    public void ChangeColor()
    {
        if (!MatchingStatus.C.IsAlive) NameColor = Color.black;
        else if (MatchingStatus.C.IsPlz) NameColor = new Color(1f, 0f, 1f, 1f);
        else if (MatchingStatus.C.IsCant) NameColor = Color.gray;
        else if (MatchingStatus.C.RedTuraa) NameColor = Color.red;
        else if (MatchingStatus.C.PartnerId != null) { NameColor = Color.green; DebuLog.C.AddDlList($"green id:{MatchingStatus.C.PartnerId}"); }
        else if (MatchingStatus.C.PinkTuraaList.Count > 0) NameColor = new Color(0.5f, 4f, 1f, 1f);
        else NameColor = Color.blue;
        ChangeColorServerRpc(NameColor.r, NameColor.g, NameColor.b, NameColor.a);
    }
    [ServerRpc]
    void ChangeColorServerRpc(float r, float g, float b, float a)
    {
        ChangeColorClientRpc(r,g, b, a);
    }

    [ClientRpc]
    void ChangeColorClientRpc(float r, float g, float b, float a)
    {
        NameColor = new Color(r, g, b, a);
    }



}
