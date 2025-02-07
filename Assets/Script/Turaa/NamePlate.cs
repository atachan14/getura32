using UnityEngine;
using TMPro;
using Unity.Netcode;

using Unity.Collections;
using System.Collections.Generic;
using System.Collections;

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
            StartCoroutine(WaitStart());
        }
    }

    IEnumerator WaitStart()
    {
        Debug.Log("WaitStart");
        yield return new WaitForSeconds(2);
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

    public void ChangeColor()
    {
       
        Debug.Log(MatchingStatus.C.PartnerId);
        if (!MatchingStatus.C.IsAlive) NameColor = Color.black;
        else if (MatchingStatus.C.IsPlz) NameColor = new Color(1f, 1f, 6f, 1f);
        else if (MatchingStatus.C.IsCant) NameColor = Color.gray;
        else if (MatchingStatus.C.IsRed) NameColor = Color.red;
        else if (MatchingStatus.C.PartnerId != 9999) { NameColor = Color.green; Debug.Log("green?"); }
        else if (MatchingStatus.C.PinkList.Count != 0) NameColor = new Color(0.7f, 1f, 3f, 1f);
        else NameColor = Color.blue;
        ChangeColorServerRpc(NameColor.r, NameColor.g, NameColor.b, NameColor.a);
    }
    [ServerRpc]
    void ChangeColorServerRpc(float r, float g, float b, float a)
    {
        ChangeColorClientRpc(r, g, b, a);
    }

    [ClientRpc]
    void ChangeColorClientRpc(float r, float g, float b, float a)
    {
        NameColor = new Color(r, g, b, a);
    }



    public string GetName()
    {
        return nameTMP.text;
    }




}
