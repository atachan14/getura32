using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OwnerPlayer : NetworkBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float newPink = 0.6f;
    private float defaultPink = 1.0f;
    private float pinkRatio = 1f;
    [SerializeField] private float stickOffset;
    private bool hasStickPoint;

    private Rigidbody2D rb;
    private Vector3 nextPos;
    private Vector3 direction;
    private bool isMoving = false;
    public GameObject Partner { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!IsOwner) OtherTuraaSOup();
    }

    void OtherTuraaSOup()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.sortingOrder = 1;
        }
    }

    void Update()
    {
        if (Partner != null) StickMove();
    }

    public void ChangePartner(GameObject newPartner)
    {
        if (Partner != null)
        {
            DebugWndow.CI.AddDlList($"Change.Partner Partner != null Partner:{Partner.GetComponent<NamePlate>().Get()}");
            ulong oldPartnerId = Partner.GetComponent<NetworkObject>().OwnerClientId;
            ChangePartnerPartnerServerRpc(oldPartnerId);
        }
        Partner = newPartner;
    }
    [ServerRpc]
    public void ChangePartnerPartnerServerRpc(ulong oldPartnerId)
    {
        DebugWndow.CI.AddDlList($"changePPSR");
        ChangePartnerPartnerClientRpc(oldPartnerId);
    }

    [ClientRpc]
    public void ChangePartnerPartnerClientRpc(ulong oldPartnerId)
    {
        DebugWndow.CI.AddDlList($"changePPCR my:{NetworkManager.Singleton.LocalClientId} , old:{oldPartnerId}");
        DebugWndow.CI.AddDlList($"Partner is {Partner == null}");

        if (NetworkManager.Singleton.LocalClientId == oldPartnerId)
        {
            DebugWndow.CI.AddDlList($"befor ChangePPCRpc.Partner:{Partner.GetComponent<NamePlate>().Get()}");
            Partner = null;
            DebugWndow.CI.AddDlList($"after ChangePPCRpc.Partner:{Partner.GetComponent<NamePlate>().Get()}");
        }
    }

    void StickMove()
    {
        Vector3 distance = Partner.transform.position - transform.position;
        if (distance.magnitude > 10f)
        {
            hasStickPoint = false;
            SetNextPosServerRpc(Partner.transform.position);
        }
        else if(!hasStickPoint)
        {
            Vector3 midpoint = (transform.position + Partner.transform.position) / 2;
            hasStickPoint = true;
            if (transform.position.x > Partner.transform.position.x)
            {
                midpoint.x += stickOffset;
                
            }
            else
            {
                midpoint.x -= stickOffset;
            }
            SetNextPosServerRpc(midpoint);
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            MoveToNextPosition();
        }
    }

    public void ClickMove(Vector3 worldPosition)
    {
        DebugWndow.CI.AddDlList($"ClickMove:{Partner==null}");
        if (Partner == null) SetNextPosServerRpc(worldPosition);
    }

    [ServerRpc]
    public void SetNextPosServerRpc(Vector3 worldPosition)
    {
        nextPos = worldPosition;
        isMoving = true;

        //rbÇÃóÕÇ∆Ç©âÒì]Ç∆Ç©é~ÇﬂÇÈÅB
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.rotation = 0f;
    }

    void MoveToNextPosition()
    {
        Vector3 direction = (nextPos - transform.position).normalized;
        float step = speed * pinkRatio * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + direction * step);

        if (Vector3.Distance(transform.position, nextPos) < 0.1f)
        {
            transform.position = nextPos;
            if (!hasStickPoint) isMoving = false;
        }
    }

    public void OnPinkSlow(int count)
    {
        for (int i = 0; i < count; i++)
        {
            pinkRatio *= newPink;
        }
        SendPinkRatioServerRpc(pinkRatio);
    }
    public void OffPinkSlow()
    {
        pinkRatio = defaultPink;
        SendPinkRatioServerRpc(pinkRatio);
    }
    [ServerRpc]
    void SendPinkRatioServerRpc(float pinkRatio)
    {
        this.pinkRatio = pinkRatio;
    }
}
