using System.Globalization;
using Unity.Netcode;
using UnityEngine.EventSystems;
using UnityEngine;

public class TentacleController : NetworkBehaviour
{
    public GameObject inactiveTentacle;
    public GameObject activeTentacle;
    private GameObject targetPlayer;


    private readonly NetworkVariable<Vector3> tentaclePosition = new NetworkVariable<Vector3>();
    private readonly NetworkVariable<Quaternion> tentacleRotation = new NetworkVariable<Quaternion>();
    private readonly NetworkVariable<float> tentacleScaleY = new NetworkVariable<float>();

    public NetworkVariable<Vector3> TentaclePosition => tentaclePosition;

    public NetworkVariable<Quaternion> TentacleRotation => tentacleRotation;

    public NetworkVariable<float> TentacleScaleY => tentacleScaleY;

    public GameObject TargetPlayer { get => targetPlayer; set => targetPlayer = value; }


    void Start()
    {
        NoContactTentacleServerRpc();
    }

    void Update()
    {
        if (IsOwner && TargetPlayer != null)
        {
            KeepTentacle();
        }
        SyncTentacle();
    }

    public void ActivateTentacle(GameObject target)
    {
        DebuLog.C.AddDlList("ActivateTentacle");
        ContactTentacleServerRpc();
        DebuLog.C.AddDlList("ActivateTentacle after ContactTentaSRPC");

        activeTentacle.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 5);
        TargetPlayer = target;

        // クライアントで計算
        Vector3 targetPosition = TargetPlayer.transform.position;
        targetPosition.z = activeTentacle.transform.position.z;

        Vector3 direction = targetPosition - activeTentacle.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float distance = direction.magnitude;
        float spriteHeight = activeTentacle.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        Quaternion calculatedRotation = Quaternion.Euler(0, 0, angle - 90);
        float calculatedScaleY = distance / spriteHeight;

        // 計算結果をサーバーに送信
        UpdateTentacleDataServerRpc(activeTentacle.transform.position, calculatedRotation, calculatedScaleY);
        DebuLog.C.AddDlList("ActivateTentacle end");

    }

    public void NoContactTentacle()
    {
        NoContactTentacleServerRpc();
    }
    public void KeepTentacle()
    {
        Vector3 targetPosition = TargetPlayer.transform.position;
        targetPosition.z = activeTentacle.transform.position.z;

        Vector3 direction = targetPosition - activeTentacle.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 触手の回転・スケール計算
        activeTentacle.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        float distance = direction.magnitude;
        float spriteHeight = activeTentacle.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        activeTentacle.transform.localScale = new Vector3(1, distance / spriteHeight, 1);

        // サーバーに同期データを送信
        UpdateTentacleDataServerRpc(transform.position, activeTentacle.transform.rotation, activeTentacle.transform.localScale.y);
    }

    [ServerRpc]
    public void UpdateTentacleDataServerRpc(Vector3 position, Quaternion rotation, float scaleY)
    {
        // 計算したデータのみを同期
        TentaclePosition.Value = position;
        TentacleRotation.Value = rotation;
        TentacleScaleY.Value = scaleY;
    }
    public void SyncTentacle()
    {
        // NetworkVariableの値を反映
        // 親のワールド座標を基準に触手の座標を更新
        activeTentacle.transform.position = transform.position;
        activeTentacle.transform.rotation = TentacleRotation.Value;
        activeTentacle.transform.localScale = new Vector3(1, TentacleScaleY.Value, 1);
    }

    [ServerRpc]
    public void ContactTentacleServerRpc()
    {
        ContactTentacleClientRpc();
    }
    [ClientRpc]
    public void ContactTentacleClientRpc()
    {
        inactiveTentacle.SetActive(false);
        activeTentacle.SetActive(true);
    }

    [ServerRpc]
    public void NoContactTentacleServerRpc()
    {
        NoContactTentacleClientRpc();
    }
    [ClientRpc]
    public void NoContactTentacleClientRpc()
    {
        inactiveTentacle.SetActive(true);
        activeTentacle.SetActive(false);
        TargetPlayer = null;
    }

}
