using UnityEngine;
using TMPro;
using Unity.Netcode;

using Unity.Collections;

public class NamePlate : NetworkBehaviour
{
    private NetworkVariable<FixedString64Bytes> playerName = new(
        default, // デフォルト値
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    [SerializeField] private  TextMeshProUGUI nameTMP;

    void Start()
    {
        if (IsOwner)
        {
            // ローカルプレイヤーの名前を設定
            string savedName = PlayerPrefs.GetString("PlayerName", "DefaultName");
            SetPlayerNameServerRpc(savedName);
        }
    }
    void Update()
    {
        // ネットワーク変数が更新されたら表示を反映
        nameTMP.text = playerName.Value.ToString();
    }

    public void SetPlayerName(string newName)
    {
        playerName.Value = newName; 
        nameTMP.text = newName;
    }

    public string GetPlayerName()
    {
        return playerName.Value.ToString();
    }

    // 名前をServerRpcで設定
    [ServerRpc]
    void SetPlayerNameServerRpc(string newName)
    {
        playerName.Value = newName; 
    }

  
}
