using UnityEngine;
using TMPro;
using Unity.Netcode;

using Unity.Collections;
using System.Collections.Generic;
using System.Collections;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UI;

public class NamePlate : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI nameTMP;
    [SerializeField] TextMeshProUGUI lpTMP;
    [SerializeField] SpriteRenderer shadow;
    [SerializeField] SpriteRenderer ownerShadow;
    public NetworkVariable<FixedString64Bytes> TuraaName { get; set; } = new NetworkVariable<FixedString64Bytes>();

    public Color NameColor
    {
        get => nameTMP.color;
        set 
        {
            nameTMP.color = value;
            shadow.color = value;
        }
    }

    int displayLp;
    int targetLp;
    public int DisplayLp
    {
        get { return displayLp; }
        set
        {
            targetLp = value;
        }
    }

    void Start()
    {
        if (IsOwner)
        {
            SetTuraaNameServerRpc(PlayerPrefs.GetString("TuraaName"));
            nameTMP.text = PlayerPrefs.GetString("TuraaName");

            ownerShadow.gameObject.SetActive(true);
            
            StartCoroutine(WaitStart());
        }
        NameColor = Color.blue;
    }

    IEnumerator WaitStart()
    {
        Debug.Log("WaitStart");
        yield return new WaitForSeconds(0.5f);
        ChangeColor();
    }

    void Update()
    {
        nameTMP.text = TuraaName.Value.ToString();

        if (displayLp == -1) return;
        if (displayLp != targetLp)
        {
            if (displayLp < targetLp) displayLp += (targetLp - displayLp) / 10;
            if (displayLp < targetLp) displayLp++;
            if (displayLp > targetLp) displayLp -= (targetLp - displayLp) / 10;
            if (displayLp > targetLp) displayLp--;

            lpTMP.text = $"{displayLp}";
            float t = Mathf.InverseLerp(0, 100, displayLp);
            lpTMP.color = Color.Lerp(Color.blue, Color.magenta, t);
        }
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
        else if (MatchingStatus.C.PartnerId != 9999) { NameColor = Color.green; }
        else if (MatchingStatus.C.PinkList.Count != 0) NameColor = new Color(0.7f, 0.0f, 0.6f, 1f);
        else NameColor = Color.blue;
        shadow.color = NameColor;
        if (IsOwner) ChangeColorServerRpc(NameColor.r, NameColor.g, NameColor.b, NameColor.a);
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

    //public void NightCalcExe()
    //{
    //    StartCoroutine(NightCalcExeCoroutine());
    //}

    //public IEnumerator NightCalcExeCoroutine()
    //{

    //    yield return null;
    //}

    public string GetName()
    {
        return nameTMP.text;
    }




}
