using NUnit.Framework.Internal;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.Rendering.DebugUI;

public class TargetInfoManager : NetworkBehaviour
{
    [SerializeField] private GameObject pinkInfo;
    [SerializeField] private GameObject redInfo;
    [SerializeField] private GameObject stickInfo;
    private GameObject[] infos;

    [SerializeField] private MatchingEffect mEffect;
    [SerializeField] private LoveCallsManage loveCalls;
    [SerializeField] tributeButtons tributeButtons;

    private GameObject myTuraa;
    private ulong myId;
    private TentacleController tentacleController;
    private MatchingStatus mStatus;

    private GameObject targetTuraa;
    ulong targetId;
    private int tribute = 0;


    private void Awake()
    {
        infos = new GameObject[] { pinkInfo, redInfo, stickInfo };
    }
    void Start()
    {
        myId = NetworkManager.Singleton.LocalClientId;
        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        tentacleController = myTuraa.GetComponent<TentacleController>();
        mStatus = myTuraa.GetComponent<MatchingStatus>();
        ChangeInfoType(pinkInfo);
    }

    private void Update()
    {
        SelectType();
    }
    public bool SetTarget(GameObject target)
    {
        if (myTuraa == target) return false;
        if (targetTuraa != null && targetTuraa == target) return true;

        targetTuraa = target;
        targetId = target.GetComponent<NetworkObject>().OwnerClientId;

        tributeButtons.SetTopInfo(targetId,target.GetComponent<NamePlate>());

        DebLog.C.AddDlList("SetTarget End");
        return true;

    }

    void SelectType()
    {
        if (mStatus.IsRed) { ChangeInfoType(redInfo); }
        else if (mStatus.PartnerId != null && mStatus.PartnerId == targetId) { ChangeInfoType(stickInfo); }
        else { ChangeInfoType(pinkInfo); };
    }

    public void ChangeInfoType(GameObject bm)
    {
        foreach (GameObject button in infos)
        {
            button.SetActive(button == bm);
        }
    }

    public void LoveCall()
    {
        mEffect.OnRedEffect(targetTuraa);
      mStatus.IsRed = true;

        LoveCallServerRpc(targetId, myId, tribute);
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoveCallServerRpc(ulong targetId, ulong senderId, int money)
    {
        DebLog.C.AddDlList($"LoveCallServerRpc:{targetId},{senderId},{money}");

        LoveCallClientRpc(targetId, senderId, money);
    }

    [ClientRpc]
    public void LoveCallClientRpc(ulong targetId, ulong senderId, int money)
    {
        DebLog.C.AddDlList("crpc");
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            DebLog.C.AddDlList("crpc2");
            loveCalls.ReceiveLoveCall(senderId, money);
        }
    }

    public void LoveCallCansell()
    {
        LoveCallCansellServerRpc(targetId, myId);
        mEffect.OffRedEffect();
        mStatus.IsRed = false;
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoveCallCansellServerRpc(ulong targetId, ulong senderId)
    {
        LoveCallCansellClientRpc(targetId, senderId);
    }
    [ClientRpc]
    public void LoveCallCansellClientRpc(ulong targetId, ulong senderId)
    {
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            loveCalls.ReceiveLoveCallCansell(senderId);
        }
    }
    public void Split()
    {
        PartnerManager.C.SplitPartnerServerRpc(myId);
        //ChangeInfoType(pinkInfo);
    }
    [ClientRpc]
    public void ReceiveOKClientRpc(ulong targetId)
    {
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            mEffect.OffRedEffect();
            mStatus.IsRed = false;
            //ChangeInfoType(stickInfo);
        }
    }
    public void ReceiveNG()
    {
        mEffect.OffRedEffect();
        mStatus.IsRed = false;
    }

}
