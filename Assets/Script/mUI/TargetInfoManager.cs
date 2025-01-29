using NUnit.Framework.Internal;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TargetInfoManager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private GameObject pinkInfo;
    [SerializeField] private GameObject redInfo;
    [SerializeField] private GameObject stickInfo;
    private GameObject[] infos;

    [SerializeField] private MatchingEffect mEffect;
    [SerializeField] private LoveCallsManage loveCalls;

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
        ChangeLoveCallButton(pinkInfo);
    }
    public void SetTarget(GameObject target)
    {
        if (targetTuraa == target || myTuraa == target) return;
        DebLog.CI.AddDlList("SetTarget return after");
        targetTuraa = target;
        targetId = target.GetComponent<NetworkObject>().OwnerClientId;
        nameTMP.text = target.GetComponent<NamePlate>().Get();
        DebLog.CI.AddDlList($"mStatus==null:{mStatus == null}");
        if (mStatus.PartnerId != null && mStatus.PartnerId == targetId) { ChangeLoveCallButton(stickInfo); } else { ChangeLoveCallButton(pinkInfo); };
        DebLog.CI.AddDlList("SetTarget End");
    }

    public void ChangeLoveCallButton(GameObject bm)
    {
        foreach (GameObject button in infos)
        {
            button.SetActive(button == bm);
        }
    }

    public void LoveCall()
    {
        mEffect.OnRedEffect(targetTuraa);
        ChangeLoveCallButton(redInfo);

        LoveCallServerRpc(targetId, myId, tribute);
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoveCallServerRpc(ulong targetId, ulong senderId, int money)
    {
        DebLog.CI.AddDlList($"LoveCallServerRpc:{targetId},{senderId},{money}");

        LoveCallClientRpc(targetId, senderId, money);
    }

    [ClientRpc]
    public void LoveCallClientRpc(ulong targetId, ulong senderId, int money)
    {
        DebLog.CI.AddDlList("crpc");
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            DebLog.CI.AddDlList("crpc2");
            loveCalls.ReceiveLoveCall(senderId, money);
        }
    }

    public void LoveCallCansell()
    {
        LoveCallCansellServerRpc(targetId, myId);
        mEffect.OffRedEffect();
        ChangeLoveCallButton(pinkInfo);
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
        ChangeLoveCallButton(pinkInfo);
    }
    [ClientRpc]
    public void ReceiveOKClientRpc(ulong targetId)
    {
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            mEffect.OffRedEffect();
            ChangeLoveCallButton(stickInfo);
        }
    }
    public void ReceiveNG()
    {
        mEffect.OffRedEffect();
        ChangeLoveCallButton(pinkInfo);
    }

}
