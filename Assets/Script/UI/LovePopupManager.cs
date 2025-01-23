using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class LovePopupManage : NetworkBehaviour
{
    [SerializeField] DebugWndow debugUI;

    [SerializeField] private GameObject self;
    [SerializeField] private LoveCallsManage loveCallsManage;

    [SerializeField] private TextMeshProUGUI senderNameTMP;
    [SerializeField] private TextMeshProUGUI moneyTMP;

    private GameObject senderId;
    private int money;
    private GameObject senderGmo;


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
        this.senderId = senderTuraa;
        this.money = money;


        
        PlayerStatus senderStatus = senderTuraa.GetComponent<PlayerStatus>();

        senderNameTMP.text = senderStatus.PlayerName.Value.ToString();

    }
    public void OKClick()
    {
        SendOKServerRpc(senderId);
        loveCallsManage.ClearLoveCallList();
    }

    public void NegClick()
    {

    }
    public void NGClick()
    {
        SendNGServerRpc(senderId);
        loveCallsManage.RemoveLoveCallList(senderId, money);
    }

    public void BlockClick()
    {

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
            ClientManager.CI.ReceiveOK();
        }
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
            ClientManager.CI.ReceiveNG();
        }
    }
}
