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

    [SerializeField] private MatchingEffect mEffect;
    [SerializeField] private LoveCallsManage loveCalls;
    [SerializeField] GameObject triBtnObject;
    [SerializeField] TopInfo topInfo;

    private GameObject myTuraa;
    private ulong myId;
    private TentacleController tentacleController;
    private MatchingStatus mStatus;

    public GameObject TargetTuraa { get; set; }
    ulong targetId;
    NamePlate targetNamePlate;


    private void Awake()
    {
        C = this;
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
        if(targetNamePlate) targetNamePlate.targetShadow.SetActive(false);


        targetId = target.GetComponent<NetworkObject>().OwnerClientId;
        TargetTuraa = target;
        targetNamePlate = TargetTuraa.GetComponent<NamePlate>();
        targetNamePlate.targetShadow.SetActive(true);

        if (TargetTuraa != null) topInfo.SetTopInfo(targetId, targetNamePlate);


        DebuLog.C.AddDlList("SetTarget End");
        return true;
    }

    void SelectType()
    {
        if (mStatus.IsRed) { ChangeInfoType(redInfo); }
        else if (mStatus.PartnerId == targetId) { ChangeInfoType(stickInfo); }
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
        mStatus.RedTarget = TargetTuraa;
        DebuLog.C.AddDlList($"LoveCall");
        LoveCallServerRpc(targetId, myId, topInfo.GetTribute());
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoveCallServerRpc(ulong targetId, ulong senderId, int money)
    {
        DebuLog.C.AddDlList($"LoveCall SRpc");
        LoveCallClientRpc(targetId, senderId, money);
    }

    [ClientRpc]
    public void LoveCallClientRpc(ulong targetId, ulong senderId, int money)
    {
        DebuLog.C.AddDlList($"LoveCall CRpc");
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            loveCalls.ReceiveLoveCall(senderId, money);
        }
    }

    public void LoveCallCansell()
    {
        LoveCallCansellServerRpc(targetId, myId);
        //mEffect.OffRedEffect();
        mStatus.RedTarget = null;
        TopInfo.C.ReleaseMinusForRed();
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
        TopInfo.C.ReleaseMinusForRed();
    }
    [ClientRpc]
    public void ReceiveOKClientRpc(ulong targetId)
    {
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            //mEffect.OffRedEffect();
            mStatus.RedTarget = null;

            //ChangeInfoType(stickInfo);
            TopInfo.C.ReleaseMinusForRed();
        }
    }
    public void ReceiveNG()
    {
        //mEffect.OffRedEffect();
        mStatus.RedTarget = null;
        TopInfo.C.ReleaseMinusForRed();
    }

    public void WisClick()
    {
        ChatDisplay.CI.WisSet(TargetTuraa);
    }

    public void Reset()
    {
        TargetTuraa = null;
        targetId = 9999;
    }
}
