using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private QolEffect QolEffect;
    [SerializeField] private GameObject targetInfo; // UIの親オブジェクト
    [SerializeField] private TargetInfoManager targetInfoScript;
    [SerializeField] private Camera myCamera;
    private bool isTakeCamera;
    private GameObject targetPlayer;
    private float scroll;

    private NetworkObject myTuraa;

    [SerializeField] private CameraController cameraController;
    private bool fixedCamera = false;
    // Start is called before the first frame update
    void Start()
    {
        targetInfo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) ClickMove(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonDown(0)) OpenInfo();
        if (Input.GetKeyDown(KeyCode.F9)) fixedCamera = !fixedCamera;

        if (!fixedCamera && !isTakeCamera)
        {
            if (Input.mousePosition.x < 0) ToMoveCamera(Vector3.left);
            if (Input.mousePosition.x > Screen.width) ToMoveCamera(Vector3.right);
            if (Input.mousePosition.y < 0) ToMoveCamera(Vector3.down);
            if (Input.mousePosition.y > Screen.height) ToMoveCamera(Vector3.up);
        }

        if (Input.GetKeyDown(KeyCode.Y)) isTakeCamera = !isTakeCamera;
        if (Input.GetKey(KeyCode.Space) || isTakeCamera) TakeCamera();

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f) ZoomCamera(scroll);
    }

    void ZoomCamera(float scroll)
    {
        myCamera.orthographicSize -= scroll * 2f;// 値を調整しつつズーム
        myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize, 2f, 20f); // 最小/最大値制限
    }

    void TakeCamera()
    {
        if (myTuraa == null)
        {
            myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject;
        }
        Vector3 takePos = myTuraa.transform.position;
        takePos.z = -10;
        myCamera.transform.position = takePos;
    }

    void ClickMove(Vector3 worldPosition)
    {
        worldPosition.z = 0; // Z座標は2Dの場合固定値にする

        QolEffect.ClickMove(worldPosition);
    }

    void OpenInfo()
    {
        Debug.Log("Open Info");

        // レイキャストを発射
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("click Info UI");
            }
            else if (hit.collider.CompareTag("CharacterClick"))
            {
                Debug.Log("click CharacterClick");

                // 対象プレイヤーを取得
                targetPlayer = hit.collider.gameObject;

                //// UIを表示
                targetInfoScript.SetTarget(targetPlayer);
                targetInfo.SetActive(true);
            }
            else
            {
                Debug.Log("click else Tag:" + hit.collider.ToString());
            }
        }

        else
        {
            HideInfoUI();
            Debug.Log("hit.collider == null");
        }
    }

    void HideInfoUI()
    {
        // UIを非表示
        targetInfo.SetActive(false);
        targetPlayer = null;
    }

    void ToMoveCamera(Vector3 direction)
    {
        cameraController.MoveCamera(direction);
    }



}

