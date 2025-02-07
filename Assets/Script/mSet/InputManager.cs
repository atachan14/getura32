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
    float scroll;
    OwnerPlayer ownerPlayer;
    TentacleController tentacleController;
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
        if (Input.GetKey(KeyCode.Space) || F8) CameraController.C.TakeCamera();

        if (!F9 && !F8) CameraController.C.SelectMoveCamera();
        if (Input.GetKeyDown(KeyCode.F9)) F9 = !F9;

        scroll = Input.GetAxis("Mouse ScrollWheel");
<<<<<<< HEAD
        if (scroll != 0f) ZoomCamera(scroll);
=======
        if (scroll != 0f) CameraController.C.ZoomCamera(scroll);
>>>>>>> 304fbeaf236ba4e1ae50fee3414d2cb882743c0e

        if (MatchingStatus.C.RedTuraa) return;
        if (Input.GetMouseButtonDown(1)) ClickMove(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonDown(0)) OpenInfo();

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

