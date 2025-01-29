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

    [SerializeField] private TextMeshProUGUI senderNameTMP;
    [SerializeField] private TextMeshProUGUI moneyTMP;

    private ulong senderId;
    private int money;
    private GameObject senderTuraa;

    void Start()
    {
        self.SetActive(false);
    }

    public void SetData(GameObject senderTuraa, int money)
    {
        this.senderTuraa = senderTuraa;
        this.money = money;
        senderId = senderTuraa.GetComponent<NetworkObject>().OwnerClientId;

        senderNameTMP.text = senderTuraa.GetComponent<NamePlate>().Get();
    }
    public void OKClick()
    {
        SendOKServerRpc(senderId);
        loveCallsManage.RemoveLoveCallList(senderTuraa);
        PartnerManager.C.ChangePartnerServerRpc(NetworkManager.Singleton.LocalClientId,senderId);
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

        DebLog.CI.AddDlList($"NGClick.senderId:{senderId}");
        SendNGServerRpc(senderId);
        loveCallsManage.RemoveLoveCallList(senderTuraa);

    }
    [ServerRpc(RequireOwnership = false)]
    void SendNGServerRpc(ulong targetId)
    {
        DebLog.CI.AddDlList($"NGClickSeverRpc.targetId:{targetId}");
        SendNGClientRpc(targetId);
    }

    [ClientRpc]
    void SendNGClientRpc(ulong targetId)
    {
        DebLog.CI.AddDlList($"NGClickClientRpc.targetId:{targetId}");
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            targetInfo.ReceiveNG();
        }
    }
    public void BlockClick()
    {

    }




}
