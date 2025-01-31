using Unity.Netcode;
using UnityEngine;

public class NightManager : NetworkBehaviour
{
    void Start()
    {
        if (IsHost) Staging();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Staging()
    {
     //   foreach(LastDayData.C.PairIdList
    }
}
