using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TopInfo : MonoBehaviour
{
    public static TopInfo C;
    [SerializeField] TextMeshProUGUI targetNameTMP;
    [SerializeField] TextMeshProUGUI lpTMP;
    [SerializeField] TextMeshProUGUI tributeTMP;
    [SerializeField] TextMeshProUGUI triTypeTMP;

    [SerializeField] Image[] triBtnImages = new Image[4];
    ulong targetId;
    bool isMinus = false;
    string type;
    public static Dictionary<ulong, int> tributeDict = new();

    public bool P0 { get; set; } = false;
    public int Tribute { get; set; }
    public bool IsRed { get; set; }

    private void Awake()
    {
        C = this;
    }
    private void Start()
    {

    }

    public void OnClickMinusTogle()
    {
        if (IsRed) return;

        isMinus = !isMinus;
        if (isMinus) { foreach (Image im in triBtnImages) im.color = new Color(248 / 255f, 165 / 255f, 98 / 255f); }
        if (!isMinus) { foreach (Image im in triBtnImages) im.color = new Color(0.5f, 0.6f, 1f); }
    }

    public void SetMinusForRed()
    {
        isMinus = false;
        triBtnImages[0].color = Color.gray;
        IsRed = true;
    }

    public void ReleaseMinusForRed()
    {
        isMinus = true;
        triBtnImages[0].color = new Color(248 / 255f, 165 / 255f, 98 / 255f);
        IsRed = false;
    }

    public void OnClickOneK() { if (isMinus) { Tribute -= 1000; } else { Tribute += 1000; } ShowTributeTMP(); }
    public void OnClickFiveK() { if (isMinus) { Tribute -= 5000; } else { Tribute += 5000; } ShowTributeTMP(); }
    public void OnClickTenK() { if (isMinus) { Tribute -= 10000; } else { Tribute += 10000; } ShowTributeTMP(); }



    public void SetTopInfo(ulong id, NamePlate targetNP)
    {
        targetId = id;
        targetNameTMP.text = targetNP.GetName();
        Tribute = tributeDict.GetValueOrDefault(targetId, 0);
        ShowTributeTMP();
    }


    void ShowTributeTMP()
    {
        DebuLog.C.AddDlList($"ShowTributeTMP:{MatchingStatus.C != null},{MatchingStatus.C.PartnerId != null}");
        if (MatchingStatus.C.PartnerId != null && targetId == MatchingStatus.C.PartnerId)
        {
            ShowStickTributeTMP();
            return;
        }

        DebuLog.C.AddDlList("ShowTributeTMP nullPass");

        if (Tribute == 0) { triTypeTMP.text = ""; tributeTMP.text = ""; }
        if (Tribute > 0) { triTypeTMP.text = "give"; tributeTMP.text = Tribute.ToString(); triTypeTMP.color = new Color(0.5f, 0.6f, 1f); tributeTMP.color = new Color(0.5f, 0.6f, 1f); }
        if (Tribute < 0) { triTypeTMP.text = "reqest"; tributeTMP.text = (-1 * Tribute).ToString(); triTypeTMP.color = new Color(245 / 255f, 125 / 255f, 64 / 255f); tributeTMP.color = new Color(245 / 255f, 125 / 255f, 64 / 255f); }
        tributeDict[targetId] = Tribute;
    }

    public void ShowStickTributeTMP()
    {
        DebuLog.C.AddDlList("ShowStickTributeTMP nullPass");
        if (Tribute == 0) { triTypeTMP.text = ""; tributeTMP.text = ""; }
        else if (Tribute < 0)
        {
            tributeTMP.text = (-1 * Tribute).ToString();
            //if (isp0)
            //{
            tributeTMP.color = new Color(245 / 255f, 125 / 255f, 64 / 255f);
            triTypeTMP.text = "get";
            triTypeTMP.color = new Color(245 / 255f, 125 / 255f, 64 / 255f);
            //}
            //else
            //{
            //    tributeTMP.color = new Color(0.5f, 0.6f, 1f);
            //    triTypeTMP.text = "give";
            //    triTypeTMP.color = new Color(0.5f, 0.6f, 1f);
            //}
        }
        else if (Tribute > 0)
        {
            tributeTMP.text = Tribute.ToString();
            //if (isp0)
            //{
            triTypeTMP.color = new Color(0.5f, 0.6f, 1f);
            triTypeTMP.text = "give";
            triTypeTMP.color = new Color(0.5f, 0.6f, 1f);
            //}
            //else
            //{
            //    tributeTMP.color = new Color(245 / 255f, 125 / 255f, 64 / 255f);
            //    triTypeTMP.text = "get";
            //    triTypeTMP.color = new Color(245 / 255f, 125 / 255f, 64 / 255f);
            //}
        }
        tributeDict[targetId] = Tribute;
    }
}
