using UnityEngine;

public class DDOL : MonoBehaviour
{
    public static DDOL CI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        CI = this;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
