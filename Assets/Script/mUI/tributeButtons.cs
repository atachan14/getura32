using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class tributeButtons : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI targetNameTMP;
    [SerializeField] TextMeshProUGUI lpTMP;
    [SerializeField] TextMeshProUGUI tributeTMP;
    [SerializeField] TextMeshProUGUI triTypeTMP;

    [SerializeField] GameObject minusTogle;
    ulong targetId;
    bool isMinus = false;
    int tribute;
    string type;
    Dictionary<ulong, int> tributeDict = new();

    public void SetTopInfo(ulong id, NamePlate targetNP)
    {
        targetId = id;
        targetNameTMP.text = targetNP.GetName();
        tribute = tributeDict.GetValueOrDefault(targetId, 0);
        UpdateTributeTMP();
    }

    public void OnClickMinusTogle()
    {
        isMinus = !isMinus;
        if (isMinus) { minusTogle.GetComponent<Image>().color = Color.blue; }
        if (!isMinus) { minusTogle.GetComponent<Image>().color = Color.red; }
    }



    public void OnClickOneK() { if (isMinus) { tribute -= 1000; } else { tribute += 1000; } UpdateTributeTMP(); }
    public void OnClickFiveK() { if (isMinus) { tribute -= 5000; } else { tribute += 5000; } UpdateTributeTMP(); }
    public void OnClickTenK() { if (isMinus) { tribute -= 10000; } else { tribute += 10000; } UpdateTributeTMP(); }
    void UpdateTributeTMP()
    {
        tributeTMP.text = tribute.ToString();
        if (tribute > 0) { triTypeTMP.text = "give"; }
        if (tribute == 0) { triTypeTMP.text = ""; tributeTMP.text = ""; }
        if (tribute < 0) { triTypeTMP.text = "req"; }
        tributeDict[targetId] = tribute;
    }
}
