using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class LovePopupManage : NetworkBehaviour
{
    [SerializeField] TargetInfoManager targetInfo;

    [SerializeField] private GameObject self;
    [SerializeField] private LoveCallsManage loveCallsManage;

    [SerializeField] private TextMeshProUGUI senderNameTMP;
    [SerializeField] private TextMeshProUGUI moneyTMP;

    private ulong senderId;
    private int money;
    private GameObject senderTuraa;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        self.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetData(GameObject senderTuraa, int money)
    {
        this.senderTuraa = senderTuraa;
        this.money = money;
        senderId = senderTuraa.GetComponent<NetworkObject>().OwnerClientId;

        senderNameTMP.text = senderTuraa.GetComponent<PlayerStatus>().PlayerName.Value.ToString();
    }
    public void OKClick()
    {
        loveCallsManage.ClearLoveCallList();
        SendOKServerRpc(senderId);
        
    }
    [ServerRpc(RequireOwnership = false)]
    void SendOKServerRpc(ulong targetId)
    {
        SendOKClientRpc(targetId);
    }

    [ClientRpc]
    void SendOKClientRpc(ulong targetId)
    {
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            targetInfo.ReceiveOK();
        }
    }
    public void NegClick()
    {

    }
    public void NGClick()
    {
        loveCallsManage.RemoveLoveCallList(senderTuraa);
        SendNGServerRpc(senderId);
        
    }
    [ServerRpc(RequireOwnership = false)]
    void SendNGServerRpc(ulong targetId)
    {
        SendNGClientRpc(targetId);
    }

    [ClientRpc]
    void SendNGClientRpc(ulong targetId)
    {
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
           targetInfo.ReceiveNG();
        }
    }
    public void BlockClick()
    {

    }

    

    
}
