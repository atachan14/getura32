using Unity.Netcode;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    public static BaseUI CI;
    void Awake()
    {
        CI = this;
        DontDestroyOnLoad(this);
    }
}
