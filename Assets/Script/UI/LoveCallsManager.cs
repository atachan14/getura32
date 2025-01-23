using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LoveCallsManage : MonoBehaviour
{
    private List<(GameObject senderTuraa, int money)> loveCallList = new List<(GameObject senderTuraa, int money)>();
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
    public void AddLoveCallList(GameObject senderTuraa, int money)
    {

        loveCallList.Add((senderTuraa, money));
        ShowLovePopups();
    }

    public void RemoveLoveCallList(GameObject senderTuraa, int money)
    {
        loveCallList.Remove((senderTuraa, money));
        ShowLovePopups();
    }

    public void ClearLoveCallList()
    {
        loveCallList.Clear();
        ShowLovePopups() ;
    }

    void ShowLovePopups()
    {
        List<GameObject> senderTuraaList = new();
        ResetLovePopups();
        for (int i = 0; i < loveCallList.Count; i++)
        {
            senderTuraaList.Add(loveCallList[i].senderTuraa);
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
        if (loveCallList.Count>0)
        {
            ClientManager.CI.haveLoveCalls(senderTuraaList);
        }
        else
        {
            ClientManager.CI.dontHaveLoveCalls();
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
        lovePopupManager.SetData(loveCallList[i].senderTuraa, loveCallList[i].money);
    }
}
