using Unity.Netcode;
using UnityEngine;

public class TimeUpLeave : NetworkBehaviour
{
    [SerializeField] float speed = 7f;
    Vector3 direction;
    bool IsP0Leaving = false;

    GameObject p0;
    bool IsP1Leaving = false;
    public Vector3 DayPos { get; set; }
    bool isSetupComeBacking = false;

    public NetworkVariable<bool> isDay = new(true);
    public bool IsDay
    {
        get => isDay.Value;
        set => isDay.Value = value;
    }

    void Update()
    {
        if (IsP0Leaving) P0Leave();
        if (IsP1Leaving) P1Leave();
        if (isSetupComeBacking) ReturningDay();
    }

    public void OnP0Leave(Vector3 direction)
    {
        this.direction = direction;
        IsP0Leaving = true;
        IsDay = false;
    }
    public void OnP1Leave(GameObject p0)
    {
        this.p0 = p0;
        IsP1Leaving = true;
        IsDay = false;
    }

    void P0Leave()
    {
        transform.position += speed * Time.deltaTime * direction;

    }
    void P1Leave()
    {
        if ((transform.position - p0.transform.position).magnitude < 3f) return;
        transform.position += 1.3f * speed * Time.deltaTime * (p0.transform.position - transform.position).normalized;
    }

    public void StopLeave()
    {
        IsP0Leaving = false;
        IsP1Leaving = false;
    }

    public void StartReturnDay()
    {
        IsDay = true;
        Vector3 direction = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f).normalized;
        transform.position = DayPos;
        transform.position += direction * 10f;
        StartReturnDayServerRpc(transform.position);
    }
    [ServerRpc]
    void StartReturnDayServerRpc(Vector3 pos)
    {
        isSetupComeBacking = true;
        transform.position = pos;
    }

    void ReturningDay()
    {
        Vector3 direction = (DayPos - transform.position).normalized;
        transform.position += speed * Time.deltaTime * direction;
        if (transform.position == DayPos) { isSetupComeBacking = false; DebuLog.C.AddDlList("SetupComeBacking end"); }
    }
}
