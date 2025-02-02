using UnityEngine;

public class AloneManager : MonoBehaviour
{
    public static AloneManager C;
    void Start()
    {
        C = this;
    }

    void Update()
    {
        
    }

    public void AloneStart()
    {
        DebuLog.C.AddDlList("alone start");
    }
}
