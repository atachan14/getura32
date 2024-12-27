using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public string Name = "player1";
    int Gold = 10000;
    int Feeling = 50;
    int CharmPower = 0;
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
