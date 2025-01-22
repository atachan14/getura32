using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LoveCallsManage : MonoBehaviour
{
    private List<(ulong senderId, int money)> loveCallList = new List<(ulong senderId, int money)>();
    [SerializeField] private GameObject[] LovePopups = new GameObject[4];

    [SerializeField] private DebugWndow debugUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddLoveCallList(ulong senderId, int money)
    {

        loveCallList.Add((senderId, money));
        ShowLovePopups();
    }

    public void RemoveLoveCallList(ulong senderId, int money)
    {
        loveCallList.Remove((senderId, money));
        ShowLovePopups();
    }

    void ShowLovePopups()
    {
        ResetLovePopups();
        for (int i = 0; i < loveCallList.Count; i++)
        {
            if (i < 4)
            {
                LovePopups[i].SetActive(true);
                SetupLovePopups(i);
            }
            else if (i < 5)
            {
                return;
            }
        }
    }

    void ResetLovePopups()
    {
        foreach (GameObject popup in LovePopups)
        {
            popup.SetActive(false);
        }
    }

    void SetupLovePopups(int i)
    {
        LovePopupManage lovePopupManager = LovePopups[i].GetComponent<LovePopupManage>();
        lovePopupManager.SetData(loveCallList[i].senderId, loveCallList[i].money);
    }
}
