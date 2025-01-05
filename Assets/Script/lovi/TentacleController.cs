using System.Globalization;
using Unity.Netcode;
using UnityEngine.EventSystems;
using UnityEngine;

public class TentacleController : NetworkBehaviour
{
    public GameObject inactiveTentacle;
    public GameObject activeTentacle;

    private GameObject targetPlayer;

    // 触手の状態を同期するためのNetworkVariable
    private readonly NetworkVariable<Vector3> tentaclePosition = new NetworkVariable<Vector3>();
    private readonly NetworkVariable<Quaternion> tentacleRotation = new NetworkVariable<Quaternion>();
    private readonly NetworkVariable<float> tentacleScaleY = new NetworkVariable<float>();

    void Start()
    {
        DeactivateTentacle();
    }

    void Update()
    {

        if (IsOwner && Input.GetMouseButtonDown(0))
        {
            ActivateTentacle();
        }

        if (IsOwner && targetPlayer != null)
        {
            KeepTentacle();
        }


        // 触手の同期情報を反映
        SyncTentacle();
    }

    public void ActivateTentacle()
    {
        activeTentacle.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 5);
        inactiveTentacle.SetActive(false);
        activeTentacle.SetActive(true);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null && hit.collider.CompareTag("CharacterClick"))
        {
            targetPlayer = hit.collider.gameObject;

            // クライアントで計算
            Vector3 targetPosition = targetPlayer.transform.position;
            targetPosition.z = activeTentacle.transform.position.z;

            Vector3 direction = targetPosition - activeTentacle.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float distance = direction.magnitude;
            float spriteHeight = activeTentacle.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

            Quaternion calculatedRotation = Quaternion.Euler(0, 0, angle - 90);
            float calculatedScaleY = distance / spriteHeight;

            // 計算結果をサーバーに送信
            UpdateTentacleDataServerRpc(activeTentacle.transform.position, calculatedRotation, calculatedScaleY);
        }
        else if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("click Info UI");
        }
        else
        {
            DeactivateTentacle();
        }
    }
    public void KeepTentacle()
    {
        Vector3 targetPosition = targetPlayer.transform.position;
        targetPosition.z = activeTentacle.transform.position.z;

        Vector3 direction = targetPosition - activeTentacle.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 触手の回転・スケール計算
        activeTentacle.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        float distance = direction.magnitude;
        float spriteHeight = activeTentacle.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        activeTentacle.transform.localScale = new Vector3(1, distance / spriteHeight, 1);

        // サーバーに同期データを送信
        UpdateTentacleDataServerRpc(activeTentacle.transform.position, activeTentacle.transform.rotation, activeTentacle.transform.localScale.y);
    }

    [ServerRpc]
    public void UpdateTentacleDataServerRpc(Vector3 position, Quaternion rotation, float scaleY)
    {
        // 計算したデータのみを同期
        tentaclePosition.Value = position;
        tentacleRotation.Value = rotation;
        tentacleScaleY.Value = scaleY;

    }

    public void SyncTentacle()
    {
        // NetworkVariableの値を反映
        activeTentacle.transform.position = tentaclePosition.Value;
        activeTentacle.transform.rotation = tentacleRotation.Value;
        activeTentacle.transform.localScale = new Vector3(1, tentacleScaleY.Value, 1);
    }

    [ServerRpc]
    public void DeactivateTentacleServerRpc()
    {
        DeactivateTentacle();
    }

    public void DeactivateTentacle()
    {
        inactiveTentacle.SetActive(true);
        activeTentacle.SetActive(false);
        targetPlayer = null;
    }
}
