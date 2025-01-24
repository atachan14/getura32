using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OwnerPlayer : NetworkBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float newPink = 0.2f;
    [SerializeField] private float defaultPink = 1.0f;
    private float pinkRatio = 1f;

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
        pinkRatio =newPink;
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

        //rbの力とか回転とか止める。
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
