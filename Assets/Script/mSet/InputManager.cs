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

    TuraaWalker ownerPlayer;
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
        ownerPlayer = myTuraa.GetComponent<TuraaWalker>();
        tentacleController = myTuraa.GetComponent<TentacleController>();
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null
            && EventSystem.current.currentSelectedGameObject.GetComponent<InputField>()) return;

        if (Input.GetKeyDown(KeyCode.F8)) F8 = !F8;
        if (Input.GetKey(KeyCode.Space) || F8) CameraController.C.TakeCamera();

        if (!F9 && !F8) SelectMoveCamera();
        if (Input.GetKeyDown(KeyCode.F9)) F9 = !F9;

        scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f) CameraController.C.ZoomCamera(scroll);

        if (IsRedStop) return;
        if (Input.GetMouseButtonDown(1)) ClickMove(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonDown(0)) OpenInfo();

    }


    void SelectMoveCamera()
    {
        if (Input.mousePosition.x < 0) CameraController.C.ToMoveCamera(Vector3.left);
        if (Input.mousePosition.x > Screen.width) CameraController.C.ToMoveCamera(Vector3.right);
        if (Input.mousePosition.y < 0) CameraController.C.ToMoveCamera(Vector3.down);
        if (Input.mousePosition.y > Screen.height) CameraController.C.ToMoveCamera(Vector3.up);
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
    

    void HideInfoUI()
    {
        targetInfo.SetActive(false);
        targetPlayer = null;
        tentacleController.NoContactTentacle();
    }

}

