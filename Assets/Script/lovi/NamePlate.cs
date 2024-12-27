using UnityEngine;
using TMPro;

public class NamePlate : MonoBehaviour
{
    public TextMeshPro playerNameText;  // TextMeshProの参照
    public PlayerStatus playerStatus;   // PlayerStatusの参照

    void Start()
    {
        // PlayerStatusが設定されていない場合はエラーチェック
        if (playerStatus != null && playerNameText != null)
        {
            // PlayerStatusのnameをTextMeshProに設定
            playerNameText.text = playerStatus.name;  // nameを表示
        }
        else
        {
            Debug.LogWarning("PlayerStatus or playerNameText is not assigned!");
        }
    }
    void Update()
    {
        if (playerStatus != null && playerNameText != null)
        {
            playerNameText.text = playerStatus.name;  // nameが変わるたびに更新
        }
    }

}
