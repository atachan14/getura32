using Unity.Netcode;
using UnityEngine;

public class TimeUpLeave : NetworkBehaviour
{
    [SerializeField] float speed = 7f;
    Vector3 direction;
    bool IsP0Leaving = false;

    GameObject p0;
    bool IsP1Leaving = false;

    void Update()
    {
        if (IsP0Leaving) P0Leave();
        if (IsP1Leaving) P1Leave();
    }

    public void OnP0Leave(Vector3 direction)
    {
        this.direction = direction;
        IsP0Leaving = true;
    }
    public void OnP1Leave(GameObject p0)
    {
        this.p0 = p0;
        IsP1Leaving = true;
    }

    void P0Leave()
    {
        transform.position += direction * speed * Time.deltaTime;

    }
    void P1Leave()
    {
        transform.position += (p0.transform.position-transform.position).normalized * speed * Time.deltaTime;
    }
}
