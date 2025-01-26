using UnityEngine;

public class ClientSetting : MonoBehaviour
{
    public static ClientSetting CI;

    public string TuraaName { get; set; } = "name";
    private void Awake()
    {
        CI = this;
        DontDestroyOnLoad(this);
    }
}
