using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LPmapManager : MonoBehaviour
{
    //DontDestroyOnLoad
    public static LPmapManager S;
    public int[,] LPmap { get; set; }
    public Dictionary<ulong,int> UlongCharmDict { get; set; } =new Dictionary<ulong,int>();

    void Awake()
    {
        S = this;
    }

    public void NewGenerate()
    {
        int rs = RoomSetting.CI.RoomSize;
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

    public void ReportCharm(ulong senderId, int charm)
    {
        Debug.Log($"myId:{NetworkManager.Singleton.LocalClientId}, senderId:{senderId}, charm:{charm}");
        UlongCharmDict[senderId] = charm;
        
    }

    public void UpdateLpMap()
    {

    }
}
