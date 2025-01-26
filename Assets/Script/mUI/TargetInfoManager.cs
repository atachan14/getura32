using NUnit.Framework.Internal;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TargetInfoManager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private GameObject loveCallButton;
    [SerializeField] private GameObject loveCallCansellButton;

    [SerializeField] private MatchingEffect mEffect;
    [SerializeField] private LoveCallsManage loveCalls;

    private GameObject myTuraa;
    private GameObject targetTuraa;
    private ulong myId;
    ulong targetId;
    private int tribute = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myId = NetworkManager.Singleton.LocalClientId;
        myTuraa = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().gameObject;
        loveCallButton.SetActive(true);
        loveCallCansellButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTarget(GameObject target)
    {
        this.targetTuraa = target;
        targetId = target.GetComponent<NetworkObject>().OwnerClientId;
        nameTMP.text = target.GetComponent<NamePlate>().Get();
    }

    public void ActiveLoveCall(bool can)
    {
        loveCallButton.SetActive(can);
        loveCallCansellButton.SetActive(!can);
    }

    public void LoveCall()
    {

        mEffect.OnRedEffect(targetTuraa);
        ActiveLoveCall(false);

        LoveCallServerRpc(targetId, myId, tribute);
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoveCallServerRpc(ulong targetId, ulong senderId, int money)
    {
        LoveCallClientRpc(targetId, senderId, money);
    }

    [ClientRpc]
    public void LoveCallClientRpc(ulong targetId, ulong senderId, int money)
    {
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
           loveCalls.ReceiveLoveCall(senderId, money);
        }
    }

    public void LoveCallCansell()
    {
        LoveCallCansellServerRpc(targetId, myId);
        mEffect.OffRedEffect();
        ActiveLoveCall(true);
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
    public void ReceiveOK()
    {
        mEffect.OffRedEffect();
        ActiveLoveCall(true);
    }
    public void ReceiveNG()
    {
        mEffect.OffRedEffect();
        ActiveLoveCall(true);
    }

}
