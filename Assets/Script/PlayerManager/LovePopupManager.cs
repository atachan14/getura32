using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class LovePopupManager : MonoBehaviour
{
    public TextMeshProUGUI senderName;
    public TMP_Text moneyText;

    [SerializeField] private DebugUI debugUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetData(ulong senderId, int money)
    {
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(senderId, out var client))
        {
            GameObject senderGmo = client.PlayerObject.gameObject;
            PlayerStatus senderStatus = senderGmo.GetComponent<PlayerStatus>();

            debugUI.AddDlList($"SetData true,senderName:{senderStatus.PlayerName.Value.ToString()}");
            this.senderName.text = senderStatus.PlayerName.Value.ToString();
        }
        else
        {
            Debug.LogWarning("指定されたClientIdのクライアントが存在しません！");
        }
        
        

    }
}
