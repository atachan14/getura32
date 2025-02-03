using Unity.Netcode.Components;
using Unity.Netcode;
using UnityEngine;

public class DayReturn : MonoBehaviour
{
    public static DayReturn C;

    private void Awake()
    {
        C = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDayReturn()
    {
        CameraController.C.DayCamera();
        ReturnSetActives();
        ComeBackDayPos();
    }
    void ReturnSetActives()
    {
        
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            GameObject turaa = client.Value.PlayerObject.gameObject;
            turaa.SetActive(true);
            turaa.GetComponent<NetworkTransform>().enabled = true;
            turaa.GetComponent<Rigidbody2D>().simulated = true;
            turaa.GetComponent<SpriteController>().ChangeSPRs_A(1f);
        }
    }
    void ComeBackDayPos()
    {
        DebuLog.C.AddDlList("ComeBackDayPos");
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            client.Value.PlayerObject.gameObject.GetComponent<TimeUpLeave>().StartReturnDay();
        }
    }
}
