using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class RobbyManager : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI robbySizeTMP;
    [SerializeField] TextMeshProUGUI timeSettingTMP;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject[] timeSetButton = new GameObject[4];
    void Start()
    {
        if (!IsHost)
        {
            startButton.SetActive(false);
            foreach (GameObject button in timeSetButton) button.SetActive(false);
        }
    }
    void Update()
    {
        if (NetworkManager.Singleton != null)
        {
            int clientCount = NetworkManager.Singleton.ConnectedClients.Count;
            robbySizeTMP.text = clientCount.ToString();
        }
    }
    public void StartExe()
    {
        RoomSetting.CI.RoomSize = int.Parse(robbySizeTMP.text);
        RoomSetting.CI.TimeSize = int.Parse(timeSettingTMP.text);
        SaveInputModeClientRpc();
        SLD.SingleLoad(SNM.Opening);
    }

    [ClientRpc]
    public void SaveInputModeClientRpc()
    {
        PlayerPrefs.SetInt("F8", InputManager.CI.F8 ? 0 : 1);
        PlayerPrefs.SetInt("F9", InputManager.CI.F9 ? 0 : 1);
    }

    [ServerRpc]
    void timeChangeServerRpc(int value)
    {
        timeChangeClientRpc(value);
    }
    [ClientRpc]
    void timeChangeClientRpc(int value)
    {
        timeSettingTMP.text = value.ToString();
    }


    public void PlusOneExe()
    {
        int value = int.Parse(timeSettingTMP.text);
        value++;
        timeChangeServerRpc(value);
    }

    public void PlusTenExe()
    {
        int value = int.Parse(timeSettingTMP.text);
        value += 10;
        timeChangeServerRpc(value);
    }

    public void MinusOneExe()
    {
        int value = int.Parse(timeSettingTMP.text);
        value--;
        timeChangeServerRpc(value);
    }

    public void MinusTenExe()
    {
        int value = int.Parse(timeSettingTMP.text);
        value -= 10;
        timeChangeServerRpc(value);
    }
}
