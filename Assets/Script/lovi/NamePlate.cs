using UnityEngine;
using TMPro;

public class NamePlate : MonoBehaviour
{
    public TextMeshProUGUI nameTMP; // TextMeshProの参照
    public string DisplayName
    {
        get => nameTMP.text;  // 現在のテキストを取得
        set => nameTMP.text = value;  // テキストを設定
    }
    public PlayerStatus playerStatus;   // PlayerStatusの参照

    void Start()
    {
        // PlayerStatusが設定されていない場合はエラーチェック
        if (playerStatus != null && nameTMP != null)
        {
            // PlayerStatusのnameをTextMeshProに設定
            nameTMP.text = playerStatus.Name.Value;  // nameを表示
            Debug.Log("playerStatus.name");
        }
        else
        {
            Debug.LogWarning("PlayerStatus or playerNameText is not assigned!");
            Debug.Log("playerStatus.name"+ playerStatus.name);
            Debug.Log("nameTMP.text:"+nameTMP.text);
        }
    }

}
