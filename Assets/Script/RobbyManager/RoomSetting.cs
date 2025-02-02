using Unity.Netcode;
using UnityEngine;

public class RoomSetting : NetworkBehaviour
{
    public static RoomSetting CI;
    public int RoomSize { get; set; }
    public int TimeSize { get; set; }
    public string Stage { get; set; }

    private void Awake()
    {
        CI = this;
        DontDestroyOnLoad(this);
    }
}
