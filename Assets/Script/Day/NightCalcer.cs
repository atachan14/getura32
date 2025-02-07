using Unity.Netcode;
using UnityEngine;

public class NightCalcer : MonoBehaviour
{
    public static NightCalcer C;
    ulong myId;

    void Awake()
    {
        C = this;
    }

    private void Start()
    {
        myId = NetworkManager.Singleton.LocalClientId;
    }

    public void StartCalc()
    {
        int partnerLp = MatchingStatus.C.PartnerTuraa.GetComponent<NamePlate>().DisplayLp;
        Gold.C.Value += PartnerManager.C.tribute;
        Feel.C.Value += partnerLp - 50;
        DebuLog.C.AddDlList("Calc BeforCharm");

        Charm.C.Value += partnerLp / 10;

        DebuLog.C.AddDlList("CalcEnd");

        DayNightController.C.ClientComeBackFlow();
    }
   
}
