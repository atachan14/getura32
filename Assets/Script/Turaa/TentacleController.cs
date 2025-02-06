using System.Globalization;
using Unity.Netcode;
using UnityEngine.EventSystems;
using UnityEngine;

public class TentacleController : NetworkBehaviour
{
    [SerializeField] MatchingStatus mStatus;
    public GameObject activeTentacle;
    private GameObject targetPlayer;
    private bool redAnimeDirection;
    [SerializeField] float redAnimeRange = 2f;
    [SerializeField] float redAnimeSpeed = 1f;


    private readonly NetworkVariable<Vector3> tentaclePosition = new NetworkVariable<Vector3>();
    private readonly NetworkVariable<Quaternion> tentacleRotation = new NetworkVariable<Quaternion>();
    private readonly NetworkVariable<float> tentacleScaleY = new NetworkVariable<float>();
    private readonly NetworkVariable<float> tentacleScaleX = new NetworkVariable<float>();

    public NetworkVariable<Vector3> TentaclePosition => tentaclePosition;

    public NetworkVariable<Quaternion> TentacleRotation => tentacleRotation;

    public NetworkVariable<float> TentacleScaleY => tentacleScaleY;

    public GameObject TargetPlayer { get => targetPlayer; set => targetPlayer = value; }

    public NetworkVariable<float> TentacleScaleX => tentacleScaleX;

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

        if (IsOwner && mStatus.RedTuraa) { RedAnimationServerRpc(); }
        if (IsOwner && !mStatus.RedTuraa) { NotRedAnimeServerRpc(); }
    }

    [ServerRpc]
    void RedAnimationServerRpc()
    {
        if (TentacleScaleX.Value > redAnimeRange) redAnimeDirection = false;
        if (TentacleScaleX.Value < -1 * redAnimeRange) redAnimeDirection = true;

        if (redAnimeDirection) TentacleScaleX.Value += redAnimeSpeed * Time.deltaTime;
        if (!redAnimeDirection) TentacleScaleX.Value -= redAnimeSpeed * Time.deltaTime;
    }

    [ServerRpc]
    void NotRedAnimeServerRpc()
    {
        TentacleScaleX.Value =1f;
    }

    public void ActivateTentacle(GameObject target)
    {
        ContactTentacleServerRpc();

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
        TentaclePosition.Value = position;
        TentacleRotation.Value = rotation;
        TentacleScaleY.Value = scaleY;
    }
    public void SyncTentacle()
    {
        activeTentacle.transform.position = transform.position;
        activeTentacle.transform.rotation = TentacleRotation.Value;
        activeTentacle.transform.localScale = new Vector3(TentacleScaleX.Value, TentacleScaleY.Value, 1);
    }

    [ServerRpc]
    public void ContactTentacleServerRpc()
    {
        ContactTentacleClientRpc();
    }
    [ClientRpc]
    public void ContactTentacleClientRpc()
    {
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
        activeTentacle.SetActive(false);
        TargetPlayer = null;
    }

}
