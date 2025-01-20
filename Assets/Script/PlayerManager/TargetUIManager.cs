using TMPro;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TargetUIManager : NetworkBehaviour
{
    private ulong targetId;
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private LoveCallWindowManager loveCalls;

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
        debugUI.AddDlList($"~~~~~");
        debugUI.AddDlList($"LoveCall start myId:{myId}");
        debugUI.AddDlList($"LoveCall start targetId:{targetId}");

        LoveCallServerRpc(targetId,myId, 0);
        
    }

    [ServerRpc]
    public void LoveCallServerRpc(ulong targetId,ulong senderId, int money)
    {
        debugUI.AddDlList($"LoveCallServerRpc senderId:{senderId}");
        debugUI.AddDlList($"LoveCallServerRpc targetId:{targetId}");
        LoveCallClientRpc(targetId,senderId, money);
    }

    [ClientRpc]
    public void LoveCallClientRpc(ulong targetId,ulong senderId, int money)
    {
        debugUI.AddDlList($"LoveCallClientRpc senderId:{senderId}");
        debugUI.AddDlList($"LoveCallClientRpc targetId:{targetId}");
        debugUI.AddDlList($"LoveCallClientRpc myId:{NetworkManager.Singleton.LocalClientId}");
        
        //targetIdÇégÇ¡ÇƒtargetIdÇæÇØÇ…ëóêMÅB
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            debugUI.AddDlList($"LoveCallClientRpc true senderId{senderId}");
            debugUI.AddDlList($"LoveCallClientRpc true targetId:{targetId}");
            debugUI.AddDlList($"LoveCallClientRpc true myId:{NetworkManager.Singleton.LocalClientId}");

            loveCalls.ReceiveLoveCall(senderId, money); 
        }
        else
        {
            debugUI.AddDlList($"LoveCallClientRpc false senderId{senderId}");
            debugUI.AddDlList($"LoveCallClientRpc false targetId:{targetId}");
            debugUI.AddDlList($"LoveCallClientRpc false myId:{NetworkManager.Singleton.LocalClientId}");

        }
    }



}
