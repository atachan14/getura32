using TMPro;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    SpriteRenderer[] mySPRs;
    TextMeshProUGUI[] myTMPs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mySPRs = GetComponentsInChildren<SpriteRenderer>();
        myTMPs = GetComponentsInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeSPRs_A(float a)
    {
        foreach (SpriteRenderer spr in mySPRs) 
        {
            Color color = spr.color;
            color.a = a;
            spr.color = color;
        }
    }
}
