using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerStatus : NetworkBehaviour
{
    private NetworkVariable<FixedString64Bytes> playerName = new();
    private NetworkVariable<int> gold = new(10000);
    private NetworkVariable<int> fine = new(50);
    private NetworkVariable<int> charm = new(0);

    public NetworkVariable<FixedString64Bytes> PlayerName { get => playerName; set => playerName = value; }
    public NetworkVariable<int> Gold { get => gold; set => gold = value; }
    public NetworkVariable<int> Fine { get => fine; set => fine = value; }
    public NetworkVariable<int> Charm { get => charm; set => charm = value; }



    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner)
        {
            string savedName = PlayerPrefs.GetString("PlayerName", "DefaultName");
            SetPlayerNameServerRpc(savedName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ServerRpc]
    void SetPlayerNameServerRpc(string newName)
    {
        PlayerName.Value = newName;
    }
}
