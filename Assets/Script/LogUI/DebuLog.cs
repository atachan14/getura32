using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DebuLog : MonoBehaviour
{
    public static DebuLog C;
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

  

    public void AddDlList(string value)
    {
        dlList.Add(value);

        if (dlList.Count > 30)
        {
            dlList.RemoveAt(0);
        }
        debugText.text = string.Join("\n", dlList);
    }
}
