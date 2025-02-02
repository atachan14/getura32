using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController C;
    public float ScrollSize = 10;
    private bool isMoving = false;
    float duration = 0.2f;
    float elapsed = 0f;

    Vector3 nightPos = new Vector3(1000, 1000, -100);

    private void Awake()
    {
        C = this;
    }


    public void MoveCamera(Vector3 direction)
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCameraSmoothly(direction));
        }
    }

    private IEnumerator MoveCameraSmoothly(Vector3 direction)
    {
        isMoving = true; 
        Vector3 startPosition = transform.position; 
        Vector3 targetPosition = startPosition + direction * ScrollSize; 

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime; 
            float t = elapsed / duration; 
            transform.position = Vector3.Lerp(startPosition, targetPosition, t); 
            yield return null; // フレームごとに待機
        }

        transform.position = targetPosition; // 最終位置を補正
        isMoving = false; 
    }

    public void NightCamera()
    {
        DebuLog.C.AddDlList($"NightCamera befor:{transform.position}");
        transform.position = nightPos;
        GetComponent<Camera>().orthographicSize = 10;
        DebuLog.C.AddDlList($"NightCamera after:{transform.position}");
    }
}