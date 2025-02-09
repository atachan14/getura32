using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RoomSetting : NetworkBehaviour
{
    public static RoomSetting CI;
    public int RoomSize { get; set; }
    public int TimeSize { get; set; }
    public Dictionary<ulong, int> IdToIndex { get; set; } = new();
    public Dictionary<int, ulong> IndexToId { get; set; } = new();
    public string Stage { get; set; }

    private void Awake()
    {
        CI = this;
        DontDestroyOnLoad(this);
    }

    public void SaveRoomSetting(int rs, int ts)
    {
        RoomSize = rs;
        TimeSize = ts;

        for (int i = 0; i < RoomSize; i++)
        {
            IdToIndex[NetworkManager.Singleton.ConnectedClientsIds[i]] = i;
            IndexToId[i] = NetworkManager.Singleton.ConnectedClientsIds[i];
        }
    }
}
