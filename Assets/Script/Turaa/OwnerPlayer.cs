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

    private Rigidbody2D rb;
    private Vector3 nextPos;
    private Vector3 direction;
    private bool isMoving = false;
    public GameObject partner { get; set; }

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


    void FixedUpdate()
    {
        if (isMoving)
        {
            MoveToNextPosition();
        }
    }

    [ServerRpc]
    public void ClickMoveServerRpc(Vector3 worldPosition)
    {
        nextPos = worldPosition;
        isMoving = true;

        //rb‚Ì—Í‚Æ‚©‰ñ“]‚Æ‚©Ž~‚ß‚éB
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
