using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class LoveCallsManage : MonoBehaviour
{
    [SerializeField] private MatchingEffect mEffect;
    [SerializeField] private GameObject[] LovePopups = new GameObject[4];

    private List<(GameObject senderTuraa, int money)> loveCallList;

    private void Start()
    {
        loveCallList = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.GetComponent<MatchingStatus>().PinkList;
    }

    public void AddLoveCallList(GameObject senderTuraa, int money)
    {
        DebuLog.C.AddDlList("AddLoveCallList");
        loveCallList.Add((senderTuraa, money));
        ShowLovePopups();
    }

    public void RemoveLoveCallList(GameObject senderTuraa)
    {
        loveCallList.RemoveAll(x => x.senderTuraa == senderTuraa);
        ShowLovePopups();
    }

    public void ClearLoveCallList()
    {
        loveCallList.Clear();
        ShowLovePopups();
    }

    void ShowLovePopups()
    {
        ResetLovePopups();
        List<GameObject> senderTuraaList = new();
        for (int i = 0; i < loveCallList.Count; i++)
        {
            senderTuraaList.Add(loveCallList[i].senderTuraa);
            if (i < 4)
            {
                DebuLog.C.AddDlList("beforSetActive");
                LovePopups[i].SetActive(true);
                DebuLog.C.AddDlList("afterSetActive");
                SetupLovePopups(i);
            }
            else if (4 <= i)
            {
                break;
            }
        }

        if (loveCallList.Count > 0)
        {
            mEffect.OnPinkEffect(senderTuraaList);
        }
        else
        {
            mEffect.OffPinkEffect();
        }
    }
    void ResetLovePopups()
    {
        foreach (GameObject popup in LovePopups) popup.SetActive(false);
    }

    void SetupLovePopups(int i)
    {
        DebuLog.C.AddDlList("SetupLovePopups");
        LovePopupManage lovePopupManager = LovePopups[i].GetComponent<LovePopupManage>();
        lovePopupManager.SetLovePopup(loveCallList[i].senderTuraa, loveCallList[i].money);
    }

    public void ReceiveLoveCall(ulong senderId, int money)
    {
        DebuLog.C.AddDlList("ReceiveLoveCall");
        GameObject senderTuraa = NetworkManager.Singleton.ConnectedClients[senderId].PlayerObject.gameObject;
        AddLoveCallList(senderTuraa, money);
    }

    public void ReceiveLoveCallCansell(ulong senderId)
    {
        GameObject senderTuraa = NetworkManager.Singleton.ConnectedClients[senderId].PlayerObject.gameObject;
        RemoveLoveCallList(senderTuraa);
    }
}
