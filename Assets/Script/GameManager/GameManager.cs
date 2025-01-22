using System.Drawing;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Inst;

    [SerializeField] private RoomSize roomSize;
    private int clientCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (Inst == null) Inst = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ClientSize();
    }

    void ClientSize()
    {
        int count = 0;
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            if (client.Value.PlayerObject != null)
            {
                count++;
            }
        }
        clientCount = count;
        roomSize.Reflection(clientCount);
    }

}
