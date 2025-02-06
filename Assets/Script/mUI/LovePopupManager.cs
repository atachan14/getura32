using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class LovePopupManage : NetworkBehaviour
{
    [SerializeField] TargetInfoManager targetInfo;

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
        gameObject.SetActive(false);
    }

    public void SetLovePopup(GameObject senderTuraa, int money)
    {
        this.senderTuraa = senderTuraa;
        this.money = money;
        senderId = senderTuraa.GetComponent<NetworkObject>().OwnerClientId;
        senderNameTMP.text = senderTuraa.GetComponent<NamePlate>().GetName();

        SetMoney();
    }

    void SetMoney()
    {
        if (money == 0) { moneyTMP.text = ""; return; }

        moneyTMP.text = money.ToString();
        if (money > 0) moneyTMP.color = new Color(245 / 255f, 125 / 255f, 64 / 255f);
        if (money < 0) moneyTMP.color = new Color(0.5f, 0.6f, 1f);

    }
    public void OKClick()
    {
        SendOKServerRpc(senderId);
        loveCallsManage.RemoveLoveCallList(senderTuraa);
        DebuLog.C.AddDlList($"OKClick:{myId},{senderId}");
        partnerManager.ChangePartnerServerRpc(myId, senderId,money);
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

        DebuLog.C.AddDlList($"NGClick.senderId:{senderId}");
        SendNGServerRpc(senderId);
        loveCallsManage.RemoveLoveCallList(senderTuraa);

    }
    [ServerRpc(RequireOwnership = false)]
    void SendNGServerRpc(ulong targetId)
    {
        DebuLog.C.AddDlList($"NGClickSeverRpc.targetId:{targetId}");
        SendNGClientRpc(targetId);
    }

    [ClientRpc]
    void SendNGClientRpc(ulong targetId)
    {
        DebuLog.C.AddDlList($"NGClickClientRpc.targetId:{targetId}");
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            targetInfo.ReceiveNG();
        }
    }
    public void BlockClick()
    {

    }




}
