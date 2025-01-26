using UnityEngine;

public class WindowManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ウィンドウサイズを指定する（例: 800×600）
        Screen.SetResolution(800, 640, FullScreenMode.Windowed);
        Application.targetFrameRate = 60;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
