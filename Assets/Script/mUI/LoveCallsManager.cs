using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class LoveCallsManage : MonoBehaviour
{
    [SerializeField] private GameObject[] LovePopups = new GameObject[4];

    private List<(GameObject senderTuraa, int money)> loveCallList;
    ulong myId;

    private void Start()
    {
        myId = NetworkManager.Singleton.LocalClientId;
        loveCallList = new();
    }

    public void AddLoveCallList(GameObject senderTuraa, int money)
    {
        DebuLog.C.AddDlList("Start AddLoveCallList");
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
        DebuLog.C.AddDlList("Start ShowLovePopups");
        ResetLovePopups();
        DebuLog.C.AddDlList("after ResetLovePopups");
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
        MatchingStatus.C.PinkTupleList = loveCallList;
        
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

    [ClientRpc]
    public void ReceiveLoveCallClientRpc(ulong targetId, ulong senderId, int money)
    {
        if (myId != targetId) return;
        DebuLog.C.AddDlList("ReceiveLoveCall");
        GameObject senderTuraa = NetworkManager.Singleton.ConnectedClients[senderId].PlayerObject.gameObject;
        AddLoveCallList(senderTuraa, money);
    }

    [ClientRpc]
    public void ReceiveLoveCallCansellClientRpc(ulong targetId, ulong senderId)
    {
        if (myId != targetId) return;
        GameObject senderTuraa = NetworkManager.Singleton.ConnectedClients[senderId].PlayerObject.gameObject;
        RemoveLoveCallList(senderTuraa);
    }
}

