using UnityEngine;

public class WindowManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �E�B���h�E�T�C�Y���w�肷��i��: 800�~600�j
        Screen.SetResolution(800, 640, FullScreenMode.Windowed);
        Application.targetFrameRate = 60;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
