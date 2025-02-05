using Unity.Netcode;
using UnityEngine;

public class LPmapManager : NetworkBehaviour
{
    public static LPmapManager S;
    public int[,] LPmap { get; set; }
    public int[] CharmArray { get; set; }

    void Awake()
    {
        S = this;
    }

    public void NewGenerate()
    {
        int rs = RoomSetting.CI.RoomSize;
        CharmArray = new int[rs];
        LPmap = new int[rs, rs];

        for (int i = 0; i < rs; i++)
        {
            for (int j = 0; j < rs; j++)
            {
                if (i == j) { LPmap[i, j] = 50; continue; }
                else { LPmap[i, j] = Random.Range(0, 101); }
                DebuLog.C.AddDlList($"i:{i} , j:{j} lp,{LPmap[i, j]} ");
            }
        }

        //‚½‚Ô‚ñ‚±‚±‚É•½“™‚É•ª”z‚·‚éˆ—‚ð’Ç‰Á‚·‚éB

    }

    public int GetLpFromMeTage(int me, int tage)
    {
        //DebuLog.C.AddDlList($"MeTageToLp me:{me} , tage:{tage} , lp:{LPmap[me, tage]} ");
        return LPmap[me, tage];
    }

    [ServerRpc(RequireOwnership = false)]
    public void ReportCharmServerRpc(ulong senderId, int charm)
    {

        CharmArray[(int)senderId] = charm;
        DebuLog.C.AddDlList($"myId:{NetworkManager.Singleton.LocalClientId}, senderId:{senderId}, charm:{charm}");
    }

    public void UpdateLpMap()
    {

    }
}
