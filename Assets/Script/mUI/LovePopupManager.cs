using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class LovePopupManage : NetworkBehaviour
{
    [SerializeField] TargetInfoManager targetInfo;

    [SerializeField] private GameObject self;
    [SerializeField] private LoveCallsManage loveCallsManage;
    [SerializeField] private PartnerManager partnerManager;

    [SerializeField] private TextMeshProUGUI senderNameTMP;
    [SerializeField] private TextMeshProUGUI moneyTMP;

    private ulong senderId;
    private int money;
    private GameObject senderTuraa;
    private ulong myId;

    void Start()
    {
        myId = NetworkManager.Singleton.LocalClientId;
        self.SetActive(false);
    }

    public void SetLovePopup(GameObject senderTuraa, int money)
    {
        this.senderTuraa = senderTuraa;
        this.money = money;
        senderId = senderTuraa.GetComponent<NetworkObject>().OwnerClientId;

        senderNameTMP.text = senderTuraa.GetComponent<NamePlate>().GetName();
    }
    public void OKClick()
    {
        SendOKServerRpc(senderId);
        loveCallsManage.RemoveLoveCallList(senderTuraa);
        DebLog.C.AddDlList($"OKClick:{myId},{senderId}");
        partnerManager.ChangePartnerServerRpc(myId,senderId);
    }
    [ServerRpc(RequireOwnership = false)]
    void SendOKServerRpc(ulong targetId)
    {
        targetInfo.ReceiveOKClientRpc(targetId);
    }

    
    public void WisClick()
    {

    }
    public void NGClick()
    {

        DebLog.C.AddDlList($"NGClick.senderId:{senderId}");
        SendNGServerRpc(senderId);
        loveCallsManage.RemoveLoveCallList(senderTuraa);

    }
    [ServerRpc(RequireOwnership = false)]
    void SendNGServerRpc(ulong targetId)
    {
        DebLog.C.AddDlList($"NGClickSeverRpc.targetId:{targetId}");
        SendNGClientRpc(targetId);
    }

    [ClientRpc]
    void SendNGClientRpc(ulong targetId)
    {
        DebLog.C.AddDlList($"NGClickClientRpc.targetId:{targetId}");
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            targetInfo.ReceiveNG();
        }
    }
    public void BlockClick()
    {

    }




}
