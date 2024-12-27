using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : NetworkBehaviour
{
    public TMP_InputField nameInputField;
    public TextMeshProUGUI displayNameText;
    public PlayerStatus playerStatus;
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();

        NetworkManager.Singleton.SceneManager.LoadScene("Test", LoadSceneMode.Single);
    }


    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void OnSubmitName()
    {
        string playerName = nameInputField.text;  // 入力された名前を取得
        playerStatus.name = playerName;  // PlayerStatesにセット

        // 必要に応じて、名前をUIに表示
        displayNameText.text = playerName;

        // 名前入力UIを非表示にするなど
        nameInputField.gameObject.SetActive(false);
    }

}