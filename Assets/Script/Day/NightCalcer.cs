using Unity.Netcode;
using UnityEngine;

public class NightCalcer : NetworkBehaviour
{
    public static NightCalcer C;
    private void Awake()
    {
        C = this;
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NightCalcStart()
    {
        if (IsHost) DaySetuper.S.DaySetupStart();
    }
}
