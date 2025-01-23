using TMPro;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TargetInfoManager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private LoveCallsManage loveCalls;
    [SerializeField] private GameObject loveCallButton;
    [SerializeField] private GameObject loveCallCansellButton;

    [SerializeField] private DebugWndow debugUI;

    private GameObject myTuraa;
    private GameObject target;
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
        this.target = target;
        targetId = target.GetComponent<NetworkObject>().OwnerClientId;
        nameTMP.text = target.GetComponent<PlayerStatus>().PlayerName.Value.ToString();
    }

    public void LoveCall()
    {
        LoveCallServerRpc(targetId, myTuraa, tribute);
        ClientManager.CI.SendLoveCall(targetId);

        loveCallButton.SetActive(false);
        loveCallCansellButton.SetActive(true);
    }

    public void LoveCallCansell()
    {
        LoveCallCansellServerRpc(targetId, myId);
        ClientManager.CI.SendLoveCallCansell();

        loveCallButton.SetActive(true);
        loveCallCansellButton.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoveCallServerRpc(ulong targetId, GameObject senderTuraa, int money)
    {
        LoveCallClientRpc(targetId, senderTuraa, money);
    }

    [ClientRpc]
    public void LoveCallClientRpc(ulong targetId, GameObject senderTuraa, int money)
    {
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            ClientManager.CI.ReceiveLoveCall(senderTuraa, money);
        }
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
            ClientManager.CI.ReceiveLoveCallCansell(senderId);
        }
    }
}
