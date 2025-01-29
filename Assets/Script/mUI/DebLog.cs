using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DebLog : MonoBehaviour
{
    public static DebLog C;
    public TMP_Text debugText;
    private List<string> dlList = new();

    private void Awake()
    {
        if (C == null) C = this;
    }

    private void Start()
    {
        dlList.Add("Debug");
        Debug.Log("dlList.Count:" + dlList.Count);
    }
    private void Update()
    {
       
    }

    public void AddDlList(string value)
    {
        dlList.Add(value);

        if (dlList.Count > 20)
        {
            dlList.RemoveAt(0);
        }
        debugText.text = string.Join("\n", dlList);
    }
}
