using Unity.Netcode;
using UnityEngine;

public class NightCalcer : MonoBehaviour
{
    public static NightCalcer C;
    void Awake()
    {
        C = this;
    }

   

    public void StartCalc()
    {
        DayNightController.C.ClientComeBackFlow();
    }
}
