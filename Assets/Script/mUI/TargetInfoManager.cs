using NUnit.Framework.Internal;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TargetInfoManager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private GameObject loveCallButton;
    [SerializeField] private GameObject cansellButton;
    [SerializeField] private GameObject splitButton;
    private GameObject[] buttons;

    [SerializeField] private MatchingEffect mEffect;
    [SerializeField] private LoveCallsManage loveCalls;

    private GameObject targetTuraa;
    private GameObject myTuraa;
    private ulong myId;
    ulong targetId;
    private int tribute = 0;

    enum ButtonMode
    {
        LoveCall,
        Cansell,
        Split
    }

    private void Awake()
    {
        buttons = new GameObject[] { loveCallButton, cansellButton, splitButton };
    }
    void Start()
    {
        myId = NetworkManager.Singleton.LocalClientId;
        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        ChangeLoveCallButton(loveCallButton);
    }
    public void SetTarget(GameObject target)
    {
        if (targetTuraa == target || myTuraa == target) return;
        targetTuraa = target;
        targetId = target.GetComponent<NetworkObject>().OwnerClientId;
        nameTMP.text = target.GetComponent<NamePlate>().Get();
        if (mEffect.Partner.GetComponent<NetworkObject>().OwnerClientId == targetId) { ChangeLoveCallButton(splitButton); } else { ChangeLoveCallButton(loveCallButton); };
    }

    public void ChangeLoveCallButton(GameObject bm)
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(button == bm);
        }
    }

    public void LoveCall()
    {
        mEffect.OnRedEffect(targetTuraa);
        ChangeLoveCallButton(cansellButton);

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
        ChangeLoveCallButton(loveCallButton);
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
        Split();
        ChangeLoveCallButton(loveCallButton);
    }


    public void ReceiveOK()
    {
        mEffect.OffRedEffect();
        mEffect.ChangePartner(targetTuraa);
        ChangeLoveCallButton(splitButton);
    }
    public void ReceiveNG()
    {
        mEffect.OffRedEffect();
        ChangeLoveCallButton(loveCallButton);
    }

}
