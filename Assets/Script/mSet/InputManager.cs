using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager CI;
    [SerializeField] private QolEffect QolEffect;
    [SerializeField] private GameObject targetInfo;
    [SerializeField] private TargetInfoManager targetInfoScript;
    private GameObject targetPlayer;
    private float scroll;
    Camera myCamera;

    OwnerPlayer ownerPlayer;
    TentacleController tentacleController;
    public bool IsRedStop { get; set; } = false;


    private NetworkObject myTuraa;
    public bool F8 { get; set; } 

    public bool F9 { get; set; } 

    void Start()
    {
        CI = this;
        targetInfo.SetActive(false);

        F8 = PlayerPrefs.GetInt("F8", 0) == 0;
        F9 = PlayerPrefs.GetInt("F9", 0) == 0;

        GameObject myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.GameObject();
        ownerPlayer = myTuraa.GetComponent<OwnerPlayer>();
        tentacleController = myTuraa.GetComponent<TentacleController>();
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null
            && EventSystem.current.currentSelectedGameObject.GetComponent<InputField>()) return;

        if (Input.GetKeyDown(KeyCode.F8)) F8 = !F8;
        if (Input.GetKey(KeyCode.Space) || F8) TakeCamera();

        if (!F9 && !F8) SelectMoveCamera();
        if (Input.GetKeyDown(KeyCode.F9)) F9 = !F9;

        scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f) ZoomCamera(scroll);

        if (IsRedStop) return;
        if (Input.GetMouseButtonDown(1)) ClickMove(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonDown(0)) OpenInfo();

    }


    void SelectMoveCamera()
    {
        if (Input.mousePosition.x < 0) ToMoveCamera(Vector3.left);
        if (Input.mousePosition.x > Screen.width) ToMoveCamera(Vector3.right);
        if (Input.mousePosition.y < 0) ToMoveCamera(Vector3.down);
        if (Input.mousePosition.y > Screen.height) ToMoveCamera(Vector3.up);
    }

    void ClickMove(Vector3 worldPosition)
    {
        worldPosition.z = 0;
        DebuLog.C.AddDlList("clickmove");

        QolEffect.ClickMove(worldPosition);
        ownerPlayer.ClickMove(worldPosition);
    }

    void OpenInfo()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null && hit.collider.CompareTag("CharacterClick"))
            {
                targetPlayer = hit.collider.gameObject;

                tentacleController.ActivateTentacle(targetPlayer);
                
                targetInfo.SetActive(targetInfoScript.SetTarget(targetPlayer));
                DebuLog.C.AddDlList("afterSetTarget");
                
            }
            else
            {
                HideInfoUI();
                tentacleController.NoContactTentacle();
            }
        }
    }
    void ZoomCamera(float scroll)
    {
       
        myCamera.orthographicSize -= scroll * 2f;
        myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize, 2f, 14f);
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
    void HideInfoUI()
    {
        targetInfo.SetActive(false);
        targetPlayer = null;
        tentacleController.NoContactTentacle();
    }

    void ToMoveCamera(Vector3 direction)
    {
        CameraController.C.MoveCamera(direction);
    }



}

