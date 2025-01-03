using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private QolEffect QolEffect;
    [SerializeField] private GameObject InfoUI; // UI�̐e�I�u�W�F�N�g
    [SerializeField] private InfoUI InfoUIscript;
    private GameObject targetPlayer;

    [SerializeField] private CameraController cameraController;
    private bool canMoveCamera = true;
    // Start is called before the first frame update
    void Start()
    {
        InfoUI.SetActive(false);
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
        // ���C�L���X�g�𔭎�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            // �q�b�g�����I�u�W�F�N�g��Player�^�O�������Ă��邩�m�F
            if (hit.collider.CompareTag("CharacterClick"))
            {
                // �Ώۃv���C���[���擾
                targetPlayer = hit.collider.gameObject;

                // UI��\�� (�q�b�g�n�_��UI��ݒ�)
                Vector3 uiPosition = hit.point;
                uiPosition.z = 0; 
                InfoUI.transform.position = uiPosition;
                InfoUIscript.SetTargetName(targetPlayer.GetComponent<NamePlate>().GetPlayerName());
                InfoUI.SetActive(true);
            }
        }
        else
        {
            HideInfoUI();
        }
    }

    void HideInfoUI()
    {
        // UI���\��
        InfoUI.SetActive(false);
    }




    void ToMoveCamera(Vector3 direction)
    {
        cameraController.MoveCamera(direction);
    }



}
