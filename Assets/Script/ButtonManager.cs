using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : NetworkBehaviour
{
    public TMP_InputField nameInputField;
    public PlayerStatus playerStatus;
    public NamePlate NamePlate;
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
        Debug.Log("OnSubmitName1 nameInputField.text:" + nameInputField.text);
        //Debug.Log(" 1NamePlate.DisplayName:" + NamePlate.DisplayName);
        string playerName = nameInputField.text;  // 入力された名前を取得
        //NamePlate.DisplayName = playerName;
        PlayerPrefs.SetString("PlayerName", playerName);
        //Debug.Log(" 2NamePlate.DisplayName:" + NamePlate.DisplayName);
        NamePlate.SetPlayerName(playerName);
        Debug.Log("NamePlate.GetName" + NamePlate.GetPlayerName());
    }

}