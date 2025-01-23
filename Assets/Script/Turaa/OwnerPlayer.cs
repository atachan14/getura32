using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OwnerPlayer : NetworkBehaviour
{
    public float speed = 4f;

    private Rigidbody2D rb;
    private Vector3 nextPos;
    private Vector3 direction;
    private bool isMoving = false;

    private bool isRedStop = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (IsOwner) MyTuraaSOup();
    }

    void MyTuraaSOup()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.sortingOrder = 1;
        }
    }

    void Update()
    {
        if (isRedStop) return;
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

    public void RedStop()
    {
        isRedStop= true;
    }
    public void RedRelease()
    {
        isRedStop= false;
    }

    public void OnPinkSlow()
    {
        isRedStop = true;
    }
    public void OffPinkSlow()
    {
        isRedStop = false;
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

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.rotation = 0f;
    }

    void MoveToNextPosition()
    {
        Vector3 direction = (nextPos - transform.position).normalized;
        float step = speed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + direction * step);

        if (Vector3.Distance(transform.position, nextPos) < 0.1f)
        {
            transform.position = nextPos; 
            isMoving = false;
        }
    }
}
