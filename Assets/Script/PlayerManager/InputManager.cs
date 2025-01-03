using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private QolEffect QolEffect;
    [SerializeField] private GameObject InfoUI; // UIの親オブジェクト
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
        worldPosition.z = 0; // Z座標は2Dの場合固定値にする

        QolEffect.ClickMove(worldPosition);
    }

    void OpenInfo()
    {
        // レイキャストを発射
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            // ヒットしたオブジェクトがPlayerタグを持っているか確認
            if (hit.collider.CompareTag("CharacterClick"))
            {
                // 対象プレイヤーを取得
                targetPlayer = hit.collider.gameObject;

                // UIを表示 (ヒット地点にUIを設定)
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
        // UIを非表示
        InfoUI.SetActive(false);
    }




    void ToMoveCamera(Vector3 direction)
    {
        cameraController.MoveCamera(direction);
    }



}
