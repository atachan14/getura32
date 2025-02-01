using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningManager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI nextTMP;

    void Start()
    {
        if (IsHost)
        {
            SetupLP();
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
                int lp = LPmapManager.S.MeTageToLp(i, j);
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
        tage.GetComponent<MatchingStatus>().DisplayLp = lp;
    }


    public void NextExe()
    {
        SLD.SingleLoad(SNM.Day);
    }
}
