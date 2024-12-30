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
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            if (Input.GetMouseButtonDown(1)) ClickMove(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }


    }

    void FixedUpdate()
    {
        if (IsOwner)
        {
            if (isMoving) MoveToNextPosServerRpc();
        }
    }
    public void ClickMove(Vector3 worldPosition)
    {
        worldPosition.z = 0; // Z座標は2Dの場合固定値にする
        nextPos = worldPosition;
        isMoving = true;
    }
    [ServerRpc]
    void MoveToNextPosServerRpc()
    {
        Vector3 direction = (nextPos - transform.position).normalized;
        float step = speed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + direction * step);

        if (Vector3.Distance(transform.position, nextPos) < 0.1f)
        {
            transform.position = nextPos; // 最終的に位置を正確に合わせる
            isMoving = false;
        }
    }

}
