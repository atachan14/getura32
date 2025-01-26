using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private DebugWndow debugUI;

    [SerializeField] private QolEffect QolEffect;
    [SerializeField] private GameObject targetInfo; // UI�̐e�I�u�W�F�N�g
    [SerializeField] private TargetInfoManager targetInfoScript;
    [SerializeField] private Camera myCamera;
    private bool isTakeCamera;
    private GameObject targetPlayer;
    private float scroll;
    public bool IsRedStop { get; set; } = false;


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
        if (Input.GetKeyDown(KeyCode.F8)) isTakeCamera = !isTakeCamera;
        if (Input.GetKey(KeyCode.Space) || isTakeCamera) TakeCamera();

        if (!fixedCamera && !isTakeCamera) SelectMoveCamera();
        if (Input.GetKeyDown(KeyCode.F9)) fixedCamera = !fixedCamera;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f) ZoomCamera(scroll);

        if (IsRedStop) return;
        if (Input.GetMouseButtonDown(1)) ClickMove(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonDown(0)) OpenInfo();

        ////debug
        //if (Input.GetKeyDown(KeyCode.V)) DebugDPPTransZ(1);
        //if (Input.GetKeyDown(KeyCode.C)) DebugDPPTransZ(-1);

    }

    //void DebugDPPTransZ(int value)
    //{
    //    NetworkObject DPPn = NetworkManager.Singleton.LocalClient.PlayerObject;
    //    GameObject DPPg = DPPn.gameObject;
    //    DPPg.transform.position += new Vector3(0, 0, value);
    //}
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

        QolEffect.ClickMove(worldPosition);
        NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<OwnerPlayer>().ClickMoveServerRpc(worldPosition);
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
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("CharacterClick"))
                {
                    // �Ώۃv���C���[���擾
                    targetPlayer = hit.collider.gameObject;

                    //// UI��\��
                    targetInfoScript.SetTarget(targetPlayer);
                    targetInfo.SetActive(true);
                }
                else
                {

                }
            }
            else
            {
                HideInfoUI();
            }
        }
    }
    void ZoomCamera(float scroll)
    {
        myCamera.orthographicSize -= scroll * 2f;// �l�𒲐����Y�[��
        myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize, 2f, 14f); // �ŏ�/�ő�l����
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
        // UI���\��
        targetInfo.SetActive(false);
        targetPlayer = null;
    }

    void ToMoveCamera(Vector3 direction)
    {
        cameraController.MoveCamera(direction);
    }



}

