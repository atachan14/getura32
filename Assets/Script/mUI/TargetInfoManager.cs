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
        ChangeLoveCallButton(pinkInfo);
    }
    public void SetTarget(GameObject target)
    {
        if (targetTuraa == target || myTuraa == target) return;
        targetTuraa = target;
        targetId = target.GetComponent<NetworkObject>().OwnerClientId;
        nameTMP.text = target.GetComponent<NamePlate>().Get();
        tentacleController.ActivateTentacle(target);
        if (mEffect.Partner.GetComponent<NetworkObject>().OwnerClientId == targetId) { ChangeLoveCallButton(stickInfo); } else { ChangeLoveCallButton(pinkInfo); };

        
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
        DebugWndow.CI.AddDlList($"LoveCallServerRpc:{targetId},{senderId},{money}");

        LoveCallClientRpc(targetId, senderId, money);
    }

    [ClientRpc]
    public void LoveCallClientRpc(ulong targetId, ulong senderId, int money)
    {
        DebugWndow.CI.AddDlList("crpc");
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            DebugWndow.CI.AddDlList("crpc2");
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
        mEffect.Split();
        ChangeLoveCallButton(pinkInfo);
    }


    public void ReceiveOK()
    {
        mEffect.OffRedEffect();
        mEffect.ChangePartner(targetTuraa);
        ChangeLoveCallButton(stickInfo);
    }
    public void ReceiveNG()
    {
        mEffect.OffRedEffect();
        ChangeLoveCallButton(pinkInfo);
    }

}
