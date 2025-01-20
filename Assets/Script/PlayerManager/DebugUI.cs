using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DebugUI : MonoBehaviour
{
    public TMP_Text debugText;
    private List<string> dlList = new();


    private void Start()
    {
        dlList.Add("Debug");
        Debug.Log("dlList.Count:"+dlList.Count);
    }
    private void Update()
    {
            debugText.text = string.Join("\n", dlList);
    }

    public void AddDlList(string value)
    {
        dlList.Add(value);

        if (dlList.Count > 25)
        {
            dlList.RemoveAt(0);
        }
    }
}
