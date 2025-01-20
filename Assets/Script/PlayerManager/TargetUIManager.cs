using TMPro;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TargetInfo : NetworkBehaviour
{
    private ulong targetId;
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private LoveCallWindowManager loveCalls;

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
    }

    public void LoveCall()
    {

        ulong myId = NetworkManager.Singleton.LocalClientId;
        LoveCallServerRpc(targetId, myId, 0);

    }

    [ServerRpc]
    public void LoveCallServerRpc(ulong targetId, ulong senderId, int money)
    {
        LoveCallClientRpc(targetId, senderId, money);
    }

    [ClientRpc]
    public void LoveCallClientRpc(ulong targetId, ulong senderId, int money)
    {
        if (NetworkManager.Singleton.LocalClientId == this.targetId)
        {
            loveCalls.ReceiveLoveCall(senderId, money); 
        }
    }



}
