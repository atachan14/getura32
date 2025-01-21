using TMPro;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TargetInfoManager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private LoveCallsManage loveCalls;

    [SerializeField] private DebugWndow debugUI;
    [SerializeField] private MatchingEffect matchingEffect;

    private GameObject target;

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
        this.target = target;
        
        nameTMP.text = target.GetComponent<PlayerStatus>().PlayerName.Value.ToString();
    }

    public void LoveCall()
    {
        ulong targetId = target.GetComponent<NetworkObject>().OwnerClientId;
        ulong myId = NetworkManager.Singleton.LocalClientId;
        LoveCallServerRpc(targetId,myId, 0);
        matchingEffect.OnRedEffect(target);
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

    void RedEffect()
    {

    }
}
