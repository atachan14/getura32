using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class LovePopupManage : MonoBehaviour
{
    [SerializeField] private GameObject self;
    [SerializeField] private TextMeshProUGUI senderName;
    [SerializeField] private TMP_Text moneyText;
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
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(senderId, out var client))
        {
            senderGmo = client.PlayerObject.gameObject;
            PlayerStatus senderStatus = senderGmo.GetComponent<PlayerStatus>();

            senderName.text = senderStatus.PlayerName.Value.ToString();
        }
        else
        {
            Debug.LogWarning("指定されたClientIdのクライアントが存在しません！");
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
        self.SetActive(false);
    }

    public void BlockClick()
    {

    }
}
