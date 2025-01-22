using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class LovePopupManage : NetworkBehaviour
{
    [SerializeField] DebugWndow debugUI;

    [SerializeField] private GameObject self;
    [SerializeField] private LoveCallsManage loveCallsManage;
    [SerializeField] private MatchingEffect matchingEffect;

    [SerializeField] private TextMeshProUGUI senderName;
    [SerializeField] private TMP_Text moneyText;

    private ulong senderId;
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

    public void SetData(ulong senderId, int money)
    {
        this.senderId = senderId;
        this.money = money;

        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(senderId, out var client))
        {
            senderGmo = client.PlayerObject.gameObject;
            PlayerStatus senderStatus = senderGmo.GetComponent<PlayerStatus>();

            senderName.text = senderStatus.PlayerName.Value.ToString();
        }
        else
        {
            Debug.LogWarning("SetData error:指定されたClientIdのクライアントが存在しません！");
        }
    }
    public void OKClick()
    {
        self.SetActive(false);

    }

    public void NegClick()
    {

    }
    public void NGClick()
    {
        SendMatchingReleaseServerRpc(senderId);
        loveCallsManage.RemoveLoveCallList(senderId, money);
    }

    public void BlockClick()
    {

    }

    [ServerRpc(RequireOwnership = false)]
    void SendMatchingReleaseServerRpc(ulong targetId)
    {
        SendMatchingReleaseClientRpc(targetId);
    }
    [ClientRpc]
    void SendMatchingReleaseClientRpc(ulong targetId)
    {
        if (NetworkManager.Singleton.LocalClientId == targetId)
        {
            matchingEffect.OffRedEffect();
        }
    }
}
