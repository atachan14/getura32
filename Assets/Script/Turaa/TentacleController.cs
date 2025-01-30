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

        // �N���C�A���g�Ōv�Z
        Vector3 targetPosition = TargetPlayer.transform.position;
        targetPosition.z = activeTentacle.transform.position.z;

        Vector3 direction = targetPosition - activeTentacle.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float distance = direction.magnitude;
        float spriteHeight = activeTentacle.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        Quaternion calculatedRotation = Quaternion.Euler(0, 0, angle - 90);
        float calculatedScaleY = distance / spriteHeight;

        // �v�Z���ʂ��T�[�o�[�ɑ��M
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

        // �G��̉�]�E�X�P�[���v�Z
        activeTentacle.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        float distance = direction.magnitude;
        float spriteHeight = activeTentacle.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        activeTentacle.transform.localScale = new Vector3(1, distance / spriteHeight, 1);

        // �T�[�o�[�ɓ����f�[�^�𑗐M
        UpdateTentacleDataServerRpc(transform.position, activeTentacle.transform.rotation, activeTentacle.transform.localScale.y);
    }

    [ServerRpc]
    public void UpdateTentacleDataServerRpc(Vector3 position, Quaternion rotation, float scaleY)
    {
        // �v�Z�����f�[�^�݂̂𓯊�
        TentaclePosition.Value = position;
        TentacleRotation.Value = rotation;
        TentacleScaleY.Value = scaleY;
    }
    public void SyncTentacle()
    {
        // NetworkVariable�̒l�𔽉f
        // �e�̃��[���h���W����ɐG��̍��W���X�V
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
