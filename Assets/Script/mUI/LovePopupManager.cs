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
        DebugWndow.CI.AddDlList($"SetData:{senderTuraa.GetComponent<NetworkObject>().OwnerClientId}, {money}");
        this.senderTuraa = senderTuraa;
        this.money = money;
        senderId = senderTuraa.GetComponent<NetworkObject>().OwnerClientId;

        DebugWndow.CI.AddDlList("jiji");
        senderNameTMP.text = senderTuraa.GetComponent<NamePlate>().Get();
        DebugWndow.CI.AddDlList("SetData End");
    }
    public void OKClick()
    {
        SendOKServerRpc(senderId);
        loveCallsManage.RemoveLoveCallList(senderTuraa);
        MatchingEffect.CI.ChangePartner(senderTuraa);
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

        DebugWndow.CI.AddDlList($"NGClick.senderId:{senderId}");
        SendNGServerRpc(senderId);
        loveCallsManage.RemoveLoveCallList(senderTuraa);

    }
    [ServerRpc(RequireOwnership = false)]
    void SendNGServerRpc(ulong targetId)
    {
        DebugWndow.CI.AddDlList($"NGClickSeverRpc.targetId:{targetId}");
        SendNGClientRpc(targetId);
    }

    [ClientRpc]
    void SendNGClientRpc(ulong targetId)
    {
        DebugWndow.CI.AddDlList($"NGClickClientRpc.targetId:{targetId}");
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            targetInfo.ReceiveNG();
        }
    }
    public void BlockClick()
    {

    }




}
