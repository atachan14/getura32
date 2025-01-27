using UnityEngine;

public class ClientSetting : MonoBehaviour
{
    public static ClientSetting CI;

    public string TuraaName { get; set; } = "name";
    public bool f9 { get; set; } = false;
    public bool f8 { get; set; } = false;
    private void Awake()
    {
        CI = this;
        DontDestroyOnLoad(this);
    }
}
