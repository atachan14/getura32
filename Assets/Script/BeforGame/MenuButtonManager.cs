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

        //NetworkManager.Singleton.SceneManager.LoadScene("Robby", LoadSceneMode.Single);
    }


    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void OnSubmitName()
    {
        string TuraaName = nameInputField.text;  // “ü—Í‚³‚ê‚½–¼‘O‚ðŽæ“¾
        ClientSetting.CI.TuraaName = TuraaName;
        robbyNamePlate.SetTMP(TuraaName);
        Debug.Log($"ClientSetting.TuraaName{ClientSetting.CI.TuraaName}");
    }

}