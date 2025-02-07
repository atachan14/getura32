using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class TuraaWalker : NetworkBehaviour
{
    [SerializeField] MatchingStatus mStatus;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float newPinkLate = 0.7f;
    private float defaultPink = 1f;
    private float pinkRatio = 1f;
    [SerializeField] private float stickOffset = 3f;

    private Rigidbody2D rb;
    private Vector3 nextPos;
    private Vector3 direction;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!IsOwner) OtherTuraaSOup();
        if (IsOwner) tag = "MyTuraa";
    }

    void OtherTuraaSOup()
    {

        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.sortingOrder += 1;
        }
    }

    void Update()
    {
        if (mStatus.PartnerTuraa != null)
        {
            StickMove();
        }
    }

    public void StickMove()
    {

        Vector3 distance = mStatus.PartnerTuraa.transform.position - transform.position;

        if (distance.magnitude > 2f)
        {
            Vector3 midpoint = (transform.position + mStatus.PartnerTuraa.transform.position) / 2;
            if (transform.position.x > mStatus.PartnerTuraa.transform.position.x)
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
        if (mStatus.PartnerTuraa == null) SetNextPosServerRpc(worldPosition);
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
            isMoving = false;
        }
    }

    public void OnPinkSlow(int count)
    {
        for (int i = 0; i < count; i++)
        {
            pinkRatio *= newPinkLate;
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
