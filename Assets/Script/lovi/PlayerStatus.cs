using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerStatus : NetworkBehaviour
{
    public NetworkVariable<int> Gold = new NetworkVariable<int>(10000);
    public NetworkVariable<int> Feeling = new NetworkVariable<int>(50);
    public NetworkVariable<int> CharmPower = new NetworkVariable<int>(0);
    public GameObject playerObject;

    

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
