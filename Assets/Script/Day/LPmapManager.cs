using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Netcode;
using UnityEngine;

public class LPmapManager : MonoBehaviour
{
    public static LPmapManager S;
    public int[,] LPmap { get; set; }
    public Dictionary<ulong, Dictionary<ulong, int>> LPdict { get; set; }
    public Dictionary<ulong, int> CharmDict { get; set; } = new Dictionary<ulong, int>();

    void Awake()
    {
        S = this;
    }

    private void Start()
    {
    }

    public void NewGenerate()
    {
        GenerateLPmap();
        GenerateLPdict();
    }

    void GenerateLPmap()
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

    void GenerateLPdict()
    {
        int rs = RoomSetting.CI.RoomSize;
        Dictionary<int, ulong> inti = RoomSetting.CI.IndexToId;
        LPdict = new();
        for (int i = 0; i < rs; i++)
        {
            LPdict[inti[i]] = new();
            for (int j = 0; j < rs; j++)
            {
                LPdict[inti[i]][inti[j]] = LPmap[i, j];
            }
        }
    }

    public int GetLpFromMeTage(int me, int tage)
    {
        //DebuLog.C.AddDlList($"MeTageToLp me:{me} , tage:{tage} , lp:{LPmap[me, tage]} ");
        return LPmap[me, tage];
    }

    public void ReportCharm(ulong senderId, int charm)
    {
        Debug.Log($"myId:{NetworkManager.Singleton.LocalClientId}, senderId:{senderId}, charm:{charm}");
        CharmDict[senderId] = charm;

    }

    public void NightReportReceive(ulong senderId, ulong partnerId)
    {

        int value = LPdict[senderId][partnerId];
        LPdict[senderId][partnerId] = 0;

        int sumCharm = 0;
        foreach (ulong i in CharmDict.Keys)
        {
            if (i == senderId) return;
            sumCharm += CharmDict[i];
        }

        foreach (ulong i in LPdict[senderId].Keys)
        {
            if (i == senderId) return;
            LPdict[senderId][i] += value * CharmDict[i] / sumCharm;
            Debug.Log($"sender,i,lp:{senderId},{i},{LPdict[senderId][i]}");
        }

    }



}
