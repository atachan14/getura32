using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DebugWndow : MonoBehaviour
{
    public static DebugWndow CI;
    public TMP_Text debugText;
    private List<string> dlList = new();

    private void Awake()
    {
        if (CI == null) CI = this;
    }

    private void Start()
    {
        dlList.Add("Debug");
        Debug.Log("dlList.Count:" + dlList.Count);
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
