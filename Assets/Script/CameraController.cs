using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController C;
   [SerializeField] Camera myCamera;
    [SerializeField] GameObject myTuraa;
    public float ScrollSize = 4;
    private bool isMoving = false;
    float duration = 0.2f;
    float elapsed = 0f;

    Vector3 nightPos = new Vector3(1000, 1000, -100);

    private void Awake()
    {
        C = this;

    }

    void Start()
    {
        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
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
    public void ZoomCamera(float scroll)
    {

        myCamera.orthographicSize -= scroll * 2f;
        myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize, 2f, 14f);
    }

    public void TakeCamera()
    {
        //if (myTuraa == null)
        //{
        //    myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject;
        //}
        Vector3 takePos = myTuraa.transform.position;
        takePos.z = -10;
        myCamera.transform.position = takePos;
    }


    public void ToMoveCamera(Vector3 direction)
    {
        CameraController.C.MoveCamera(direction);
    }
    public void NightCamera()
    {
        transform.position = nightPos;
        GetComponent<Camera>().orthographicSize = 10;
    }

    public void DayCamera()
    {
        Vector3 pos = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.GetComponent<TimeUpLeave>().DayPos;

        pos.z = -100;
        transform.position = pos;
        GetComponent<Camera>().orthographicSize = 15;
    }

    public void ForcusAloner(ulong id)
    {
        transform.position = NetworkManager.Singleton.ConnectedClients[id].PlayerObject.gameObject.transform.position;
    }
}