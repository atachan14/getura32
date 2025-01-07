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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
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
        rb.rotation = 0f; // これで Z 回転をリセット
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
