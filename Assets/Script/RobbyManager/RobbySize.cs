using System.Drawing;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class RoomSize : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI robbySizeTMP;
    [SerializeField] private RoomSetting RoomSetting;
   
    void Update()
    {
        {
            if (NetworkManager.Singleton != null)
            {
                int clientCount = NetworkManager.Singleton.ConnectedClients.Count;
                robbySizeTMP.text = clientCount.ToString();
                RoomSetting.RoomSize = clientCount;
            }
        }
    }


}
