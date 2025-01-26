using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerStatus : NetworkBehaviour
{
    //private NetworkVariable<FixedString64Bytes> PlayerName { get; set; } = new();
    
    //// Start is called before the first frame update
    //void Start()
    //{
    //    if (IsOwner)
    //    {
    //        string savedName = PlayerPrefs.GetString("PlayerName", "DefaultName");
    //        SetPlayerNameServerRpc(savedName);
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    //[ServerRpc]
    //void SetPlayerNameServerRpc(string newName)
    //{
    //    PlayerName.Value = newName;
    //}
}
