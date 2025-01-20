using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LoveCallsManager : MonoBehaviour
{
    private List<(ulong senderId, int money)> loveCallList = new List<(ulong senderId, int money)>();
    [SerializeField] private GameObject[] LovePopups = new GameObject[4];

    [SerializeField] private DebugUI debugUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ReceiveLoveCall(ulong senderId, int money)
    {
        debugUI.AddDlList($"ReceiveLoveCall , senderId:{senderId}");

        loveCallList.Add((senderId, money));
        ShowLovePopups();
    }

    void ShowLovePopups()
    {
        debugUI.AddDlList($"ShowLovePopups loveCallList.Count:{loveCallList.Count}");
        for (int i = 0; i < loveCallList.Count; i++)
        {
            if (i < 4)
            {
                debugUI.AddDlList($"ShowLovePopups true , senderId:{loveCallList[i].senderId}");
                LovePopups[i].SetActive(true);
                debugUI.AddDlList($"ShowLovePopups SetActive end , senderId:{loveCallList[i].senderId}");
                SetupLovePopups(i);
                debugUI.AddDlList($"ShowLovePopups SetupLovePopups end , senderId:{loveCallList[i].senderId}");

            }
            else if (i < 5)
            {
                return;
            }
        }
    }

    void SetupLovePopups(int i)
    {
        LovePopupManager lovePopupManager = LovePopups[i].GetComponent<LovePopupManager>();
        lovePopupManager.SetData(loveCallList[i].senderId, loveCallList[i].money);
    }
}
