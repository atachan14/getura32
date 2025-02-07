using NUnit.Framework.Internal;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public class TargetInfoManager : NetworkBehaviour
{
    public static TargetInfoManager C;
    [SerializeField] private GameObject pinkInfo;
    [SerializeField] private GameObject redInfo;
    [SerializeField] private GameObject stickInfo;
    private GameObject[] infos;

    [SerializeField] private LoveCallsManage loveCalls;
    [SerializeField] GameObject triBtnObject;

    private GameObject myTuraa;
    private ulong myId;
    private MatchingStatus mStatus;

    private GameObject targetTuraa;
    ulong targetId;


    private void Awake()
    {
        C = this;
        infos = new GameObject[] { pinkInfo, redInfo, stickInfo };
    }
    void Start()
    {
        myId = NetworkManager.Singleton.LocalClientId;
        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        mStatus = myTuraa.GetComponent<MatchingStatus>();
        ChangeInfoType(pinkInfo);
    }

    private void Update()
    {
        SelectType();

    }
    public bool SetTarget(GameObject target)
    {
        DebuLog.C.AddDlList("SetTarget Start");
        if (targetTuraa != null && targetTuraa == target) return true;
        DebuLog.C.AddDlList("SetTarget ifPass");

        targetId = target.GetComponent<NetworkObject>().OwnerClientId;
        targetTuraa = target;

        if (targetTuraa != null) TopInfo.C.SetTopInfo(targetId, targetTuraa.GetComponent<NamePlate>());


        DebuLog.C.AddDlList("SetTarget End");
        return true;
    }

    void SelectType()
    {
        if (mStatus.RedTuraa) { ChangeInfoType(redInfo); }
        else if (mStatus.PartnerId != null && mStatus.PartnerId == targetId) { ChangeInfoType(stickInfo); }
        else { ChangeInfoType(pinkInfo); };
    }

    public void ChangeInfoType(GameObject bm)
    {
        foreach (GameObject button in infos)
        {
            button.SetActive(button == bm);
        }

        if (bm == stickInfo) { triBtnObject.SetActive(false); } else { triBtnObject.SetActive(true); }
        if (bm == redInfo) { TopInfo.C.SetMinusForRed(); }

    }

    public void LoveCall()
    {
        MatchingStatus.C.RedTuraa = targetTuraa;
        LoveCallServerRpc(targetId, myId, TopInfo.C.GetTribute());
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoveCallServerRpc(ulong targetId, ulong senderId, int money)
    {
       loveCalls.ReceiveLoveCallClientRpc(targetId, senderId, money);
    }


    public void LoveCallCansell()
    {
        LoveCallCansellServerRpc(targetId, myId);
        MatchingStatus.C.RedTuraa = null;
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoveCallCansellServerRpc(ulong targetId, ulong senderId)
    {
        loveCalls.ReceiveLoveCallCansellClientRpc(targetId, senderId);
    }
    //[ClientRpc]
    //public void LoveCallCansellClientRpc(ulong targetId, ulong senderId)
    //{
    //    if (NetworkManager.Singleton.LocalClientId == targetId)
    //    {
    //        loveCalls.ReceiveLoveCallCansell(senderId);
    //    }
    //}
    public void Split()
    {
        SplitManager.C.Split();
    }
    [ClientRpc]
    public void ReceiveOKClientRpc(ulong targetId)
    {
        if (myId != targetId) return;
        SplitManager.C.Split();
        MatchingStatus.C.PartnerId = targetId;
    }
    [ClientRpc]
    public void ReceiveNGClientRpc(ulong ntrId)
    {
        if (myId == ntrId) return;
        mStatus.RedTuraa = null;

    }

}
