using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerStatus : NetworkBehaviour
{
    public NetworkVariable<string> Name = new NetworkVariable<string>("player");
    public NetworkVariable<int> Gold = new NetworkVariable<int>(10000);
    public NetworkVariable<int> Feeling = new NetworkVariable<int>(50);
    public NetworkVariable<int> CharmPower = new NetworkVariable<int>(0);
    List<GameObject> loviList;
    Dictionary<GameObject,int> meltedLevels = new Dictionary<GameObject, int>();    


    
    // Start is called before the first frame update
    void Start()
    {
       // setUpMeltedLevels();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
