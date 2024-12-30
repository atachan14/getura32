using UnityEngine;
using TMPro;
using Unity.Netcode;

using Unity.Collections;

public class NamePlate : NetworkBehaviour
{
    // FixedString64Bytes を使用
    private NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>(
        default(FixedString64Bytes), // デフォルト値
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    public TextMeshProUGUI nameTMP;

    void Start()
    {
        if (IsOwner)
        {
            // ローカルプレイヤーの名前を設定
            string savedName = PlayerPrefs.GetString("PlayerName", "DefaultName");
            SetPlayerNameServerRpc(savedName);
        }
    }

    // 名前をServerRpcで設定
    [ServerRpc]
    void SetPlayerNameServerRpc(string newName)
    {
        playerName.Value = newName; // サーバーで値を更新
    }

    void Update()
    {
        // ネットワーク変数が更新されたら表示を反映
        nameTMP.text = playerName.Value.ToString();
    }
}
