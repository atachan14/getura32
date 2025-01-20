using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private QolEffect QolEffect;
    [SerializeField] private GameObject targetInfo; // UI�̐e�I�u�W�F�N�g
    [SerializeField] private TargetUIManager targetInfoScript;
    private GameObject targetPlayer;

    [SerializeField] private CameraController cameraController;
    private bool canMoveCamera = true;
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

        if (canMoveCamera)
        {
            if (Input.mousePosition.x < 0) ToMoveCamera(Vector3.left);
            if (Input.mousePosition.x > Screen.width) ToMoveCamera(Vector3.right);
            if (Input.mousePosition.y < 0) ToMoveCamera(Vector3.down);
            if (Input.mousePosition.y > Screen.height) ToMoveCamera(Vector3.up);
            if (Input.GetKeyDown(KeyCode.F9)) canMoveCamera = false;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F9)) canMoveCamera = true;
        }


    }

    void ClickMove(Vector3 worldPosition)
    {
        worldPosition.z = 0; // Z���W��2D�̏ꍇ�Œ�l�ɂ���

        QolEffect.ClickMove(worldPosition);
    }

    void OpenInfo()
    {
        Debug.Log("Open Info");


        // ���C�L���X�g�𔭎�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            // �q�b�g�����I�u�W�F�N�g��Player�^�O�������Ă��邩�m�F
            if (hit.collider.CompareTag("CharacterClick"))
            {
                Debug.Log("click CharacterClick");

                // �Ώۃv���C���[���擾
                targetPlayer = hit.collider.gameObject;

                //// UI��\��
                targetInfoScript.SetTarget(targetPlayer);
                targetInfo.SetActive(true);

            }
            else
            {
                Debug.Log("click else Tag:"+hit.collider.ToString());
            }
        }
        else if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("click Info UI");
        }
        else
        {
            HideInfoUI();
            Debug.Log("hit.collider == null");
        }
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

