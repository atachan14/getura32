using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class TentacleController : NetworkBehaviour
{
    public GameObject inactiveTentacle;
    public GameObject activeTentacle;
    private GameObject targetPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DeactivateTentacle();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            if (Input.GetMouseButtonDown(0)) ActivateTentacle();
            if (targetPlayer != null) KeepTentacle();
        }
    }

    public void KeepTentacle()
    {
        Vector3 targetPosition = targetPlayer.transform.position;
        targetPosition.z = activeTentacle.transform.position.z;

        Vector3 direction = targetPosition - activeTentacle.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        activeTentacle.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // 距離を正確に計算して画像を引き延ばす
        float distance = direction.magnitude;
        float spriteHeight = activeTentacle.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        activeTentacle.transform.localScale = new Vector3(1, distance / spriteHeight, 1);
    }

    public void ActivateTentacle()
    {
        // 非アクティブ触手を隠す
        inactiveTentacle.SetActive(false);

        // アクティブ触手を表示
        activeTentacle.SetActive(true);

        // マウス位置でのRaycastを実行
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null&& hit.collider.CompareTag("CharacterClick"))
        {
            // 触手のターゲット位置を取得 (ヒット地点)
            Vector3 targetPosition = hit.point;
            targetPosition.z = activeTentacle.transform.position.z; // Z座標を揃える

            // 対象の方向を計算
            Vector3 direction = targetPosition - activeTentacle.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            activeTentacle.transform.rotation = Quaternion.Euler(0, 0, angle); // 必要なら補正
         

            // 距離を正確に計算して画像を引き延ばす
            float distance = direction.magnitude;
            float spriteHeight = activeTentacle.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
            activeTentacle.transform.localScale = new Vector3(1, distance / spriteHeight, 1);
            
            targetPlayer = hit.collider.gameObject;
        }
        else if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("click Info UI");
        }
        else
        {
            // マウスがヒットしなければ触手を非表示
            DeactivateTentacle();
            Debug.Log("No hit detected. Tentacle deactivated.");
        }
    }






    public void DeactivateTentacle()
    {
        inactiveTentacle.SetActive(true);
        activeTentacle.SetActive(false);
        targetPlayer = null;
    }
}