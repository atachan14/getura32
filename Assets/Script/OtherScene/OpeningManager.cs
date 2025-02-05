using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningManager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI nextTMP;
    [SerializeField] int startGold = 50000;
    [SerializeField] int startFeel = 50;
    [SerializeField] int startCharm = 10;

    void Start()
    {
        if (IsHost)
        {
            SetupLP();
            SetupStatus();
        }
    }

    void SetupLP()
    {
        LPmapManager.S.NewGenerate();
        DeliverLP();
    }

    void DeliverLP()
    {
        int rs = RoomSetting.CI.RoomSize;
        for (int i = 0; i < rs; i++)
        {
            for (int j = 0; j < rs; j++)
            {
                if (i == j) continue;
                int lp = LPmapManager.S.GetLpFromMeTage(i, j);
                DeliverLPClientRpc(i, j, lp);
            }
        }
        DebuLog.C.AddDlList("DeliverLP end");
    }

    [ClientRpc]
    void DeliverLPClientRpc(int me, int target, int lp)
    {
        int myIntId = (int)NetworkManager.Singleton.LocalClientId;
        if (myIntId != me) return;
        if (!NetworkManager.Singleton.ConnectedClients.ContainsKey((ulong)target)) DebuLog.C.AddDlList($"DliverLPCRpc:???tage unknown???");
        
        GameObject tage = NetworkManager.Singleton.ConnectedClients[(ulong)target].PlayerObject.gameObject;
        tage.GetComponent<NamePlate>().DisplayLp = lp;
    }

    void SetupStatus()
    {
        DeliverStatusClientRpc(startGold, startFeel, startCharm);
    }

    [ClientRpc]
    void DeliverStatusClientRpc(int gold,int feel,int charm)
    {
        Gold.C.Value = gold;
        Feel.C.Value = feel;
        Charm.C.Value = charm;
    }




    public void NextExe()
    {
        SLD.SingleLoad(SNM.Day);
    }
}
