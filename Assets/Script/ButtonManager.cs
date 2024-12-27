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
        string playerName = nameInputField.text;  // ���͂��ꂽ���O���擾
        playerStatus.name = playerName;  // PlayerStates�ɃZ�b�g

        // �K�v�ɉ����āA���O��UI�ɕ\��
        displayNameText.text = playerName;

        // ���O����UI���\���ɂ���Ȃ�
        nameInputField.gameObject.SetActive(false);
    }

}