using TMPro;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TargetUIManager : NetworkBehaviour
{
    private ulong targetId;
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private LoveCallsManager loveCalls;

    [SerializeField] private DebugUI debugUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTarget(GameObject target)
    {
        targetId = target.GetComponent<NetworkObject>().OwnerClientId;
        nameTMP.text = target.GetComponent<PlayerStatus>().PlayerName.Value.ToString();
        debugUI.AddDlList($"SetTarget Id:{targetId} , Name:{nameTMP.text}");
    }

    public void LoveCall()
    {
        ulong myId = NetworkManager.Singleton.LocalClientId;
        LoveCallServerRpc(targetId,myId, 0);
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoveCallServerRpc(ulong targetId,ulong senderId, int money)
    {
        LoveCallClientRpc(targetId,senderId, money);
    }

    [ClientRpc]
    public void LoveCallClientRpc(ulong targetId,ulong senderId, int money)
    {
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            loveCalls.ReceiveLoveCall(senderId, money); 
        }
    }
}
