using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonManage : NetworkBehaviour
{
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] MenuNamePlate robbyNamePlate;
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        SLD.SingleLoad(SNM.Robby);

    }


    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void OnSubmitName()
    {
        string TuraaName = nameInputField.text; 
        PlayerPrefs.SetString("TuraaName", TuraaName);
        robbyNamePlate.SetTMP(TuraaName);

        //Debug
        PlayerPrefs.SetInt("F8", 1);
        PlayerPrefs.SetInt("F9", 0);
    }

}