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
    public static Dictionary<ulong, int> tributeDict = new();



    private void Awake()
    {
        C = this;
    }
   

    public void OnClickMinusTogle()
    {
        if (MatchingStatus.C.RedTuraa) return;

        isMinus = !isMinus;
        if (isMinus) { foreach (Image im in triBtnImages) im.color = new Color(248 / 255f, 165 / 255f, 98 / 255f); }
        if (!isMinus) { foreach (Image im in triBtnImages) im.color = new Color(0.5f, 0.6f, 1f); }
    }

    public void MinusForRed(bool b)
    {
        if (b) SetMinusForRed();
        else ReleaseMinusForRed();
    }

    public void SetMinusForRed()
    {
        isMinus = false;
        triBtnImages[0].color = Color.gray;
    }

    public void ReleaseMinusForRed()
    {
        OnClickMinusTogle();
        OnClickMinusTogle();
    }

    public void OnClickOneK() { if (isMinus) { tributeDict[targetId] -= 1000; } else { tributeDict[targetId] += 1000; } ShowTributeTMP(); }
    public void OnClickFiveK() { if (isMinus) { tributeDict[targetId] -= 5000; } else { tributeDict[targetId] += 5000; } ShowTributeTMP(); }
    public void OnClickTenK() { if (isMinus) { tributeDict[targetId] -= 10000; } else { tributeDict[targetId] += 10000; } ShowTributeTMP(); }



    public void SetTopInfo(ulong id, NamePlate targetNP)
    {
        DebuLog.C.AddDlList($"SetTopInfo");
        targetId = id;
        targetNameTMP.text = targetNP.GetName();

        ShowTributeTMP();
    }

    void ShowTributeTMP()
    {
        DebuLog.C.AddDlList($"ShowTributeTMP");
        int tribute = tributeDict.GetValueOrDefault(targetId, 0);

        DebuLog.C.AddDlList($"ShowTributeTMP:{MatchingStatus.C != null},{MatchingStatus.C.PartnerId != null}");
        if (MatchingStatus.C.PartnerId != null && targetId == MatchingStatus.C.PartnerId)
        {
            ShowStickTributeTMP();
            return;
        }

        DebuLog.C.AddDlList("ShowTributeTMP nullPass");

        if (tribute == 0) { triTypeTMP.text = ""; tributeTMP.text = ""; }
        if (tribute > 0) { triTypeTMP.text = "give"; tributeTMP.text = tribute.ToString(); triTypeTMP.color = new Color(0.5f, 0.6f, 1f); tributeTMP.color = new Color(0.5f, 0.6f, 1f); }
        if (tribute < 0) { triTypeTMP.text = "reqest"; tributeTMP.text = (-1 * tribute).ToString(); triTypeTMP.color = new Color(245 / 255f, 125 / 255f, 64 / 255f); tributeTMP.color = new Color(245 / 255f, 125 / 255f, 64 / 255f); }
        tributeDict[targetId] = tribute;
    }

    public void ShowStickTributeTMP()
    {
        int tribute = tributeDict.GetValueOrDefault(targetId, 0);
        DebuLog.C.AddDlList("ShowStickTributeTMP nullPass");
        if (tribute == 0) { triTypeTMP.text = ""; tributeTMP.text = ""; }
        else if (tribute < 0)
        {
            tributeTMP.text = (-1 * tribute).ToString();
            tributeTMP.color = new Color(245 / 255f, 125 / 255f, 64 / 255f);
            triTypeTMP.text = "get";
            triTypeTMP.color = new Color(245 / 255f, 125 / 255f, 64 / 255f);

        }
        else if (tribute > 0)
        {
            tributeTMP.text = tribute.ToString();
            tributeTMP.color = new Color(0.5f, 0.6f, 1f);
            triTypeTMP.text = "give";
            triTypeTMP.color = new Color(0.5f, 0.6f, 1f);
        }
        tributeDict[targetId] = tribute;
    }

    public int GetTribute()
    {
        return tributeDict[targetId];
    }

    public int GetTributeFromId(ulong id)
    {
        return tributeDict[id];
    }
}
