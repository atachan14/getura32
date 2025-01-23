using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OwnerPlayer : NetworkBehaviour
{
    public float speed = 4f;
    private float pinkRatio = 1f;
    private float redRatio = 1f;

    private Rigidbody2D rb;
    private Vector3 nextPos;
    private Vector3 direction;
    private bool isMoving = false;

    public bool IsRedStop { get; set; } = false;

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
        if (IsRedStop) return;
        if (IsOwner)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 clickWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                clickWorldPos.z = 0;
                ClickMoveServerRpc(clickWorldPos);
            }
        }
    }


    public void OnPinkSlow()
    {
        pinkRatio = 0.2f;
        SendPinkRatioServerRpc(pinkRatio);
        DebugWndow.CI.AddDlList($"----OnPinkSlow:{pinkRatio}");
    }
    public void OffPinkSlow()
    {
        pinkRatio = 1;
        SendPinkRatioServerRpc(pinkRatio);
    }
    [ServerRpc]
    void SendPinkRatioServerRpc(float pinkRatio)
    {
        this.pinkRatio = pinkRatio;
    }


    void FixedUpdate()
    {
        if (isMoving)
        {
            MoveToNextPosition();
        }
    }

    [ServerRpc]
    void ClickMoveServerRpc(Vector3 worldPosition)
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
}
