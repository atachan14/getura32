using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI targetNameTMP;
    [SerializeField] TextMeshProUGUI lpTMP;
    [SerializeField] TextMeshProUGUI tributeTMP;
    [SerializeField] TextMeshProUGUI triTypeTMP;

    [SerializeField] Image[] tributeButtons=new Image[4];
    ulong targetId;
    bool isMinus = false;
    int tribute;
    string type;
    Dictionary<ulong, int> tributeDict = new();

    public int Tribute { get => tribute; set => tribute = value; }

    public void SetTopInfo(ulong id, NamePlate targetNP)
    {
        targetId = id;
        targetNameTMP.text = targetNP.GetName();
        Tribute = tributeDict.GetValueOrDefault(targetId, 0);
        UpdateTributeTMP();
    }

    public void OnChangeMinusTogle(bool b)
    {
        isMinus = !b;
    }

    public void OnClickMinusTogle()
    {
        isMinus = !isMinus;
        if (isMinus) { foreach(Image im in tributeButtons) im.color = new Color(0.5f, 0.6f, 1f); }
        if (!isMinus) { foreach (Image im in tributeButtons) im.color = new Color(248/255f, 165/255f, 98/255f); }
    }



    public void OnClickOneK() { if (isMinus) { Tribute -= 1000; } else { Tribute += 1000; } UpdateTributeTMP(); }
    public void OnClickFiveK() { if (isMinus) { Tribute -= 5000; } else { Tribute += 5000; } UpdateTributeTMP(); }
    public void OnClickTenK() { if (isMinus) { Tribute -= 10000; } else { Tribute += 10000; } UpdateTributeTMP(); }
    void UpdateTributeTMP()
    {
        tributeTMP.text = Tribute.ToString();
        if (Tribute > 0) { triTypeTMP.text = "give"; }
        if (Tribute == 0) { triTypeTMP.text = ""; tributeTMP.text = ""; }
        if (Tribute < 0) { triTypeTMP.text = "reqest"; }
        tributeDict[targetId] = Tribute;
    }
}
