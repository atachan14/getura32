using UnityEngine;

public class RoomSetting : MonoBehaviour
{
    private static RoomSetting SI;
    public int RoomSize { get; set; }
    public int TimeSize { get; set; }
    public string Stage { get; set; }

    private void Awake()
    {
        SI = this;
        DontDestroyOnLoad(this);
    }
}
