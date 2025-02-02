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

    public NetworkVariable<bool> isDay = new NetworkVariable<bool>(true);
    public bool IsDay
    {
        get => isDay.Value;
        set => isDay.Value = value;
    }

    public Vector3 DayPos { get; set; }
    bool isSetupComeBacking = false;


    void Update()
    {
        if (IsP0Leaving) P0Leave();
        if (IsP1Leaving) P1Leave();
        if (isSetupComeBacking) SetupComeBacking();
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
        transform.position += direction * speed * Time.deltaTime;

    }
    void P1Leave()
    {
        if ((transform.position - p0.transform.position).magnitude < 3f) return;
        transform.position += (p0.transform.position - transform.position).normalized * speed * 1.3f * Time.deltaTime;
    }

    public void StopLeave()
    {
        IsP0Leaving = false;
        IsP1Leaving = false;
    }

    public void StartSetupComeBack()
    {
<<<<<<< HEAD
        IsDay = true;
=======
>>>>>>> d63f20ddf1c1ca6ddbac9f6def105e1c4ea72960
        Vector3 direction = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f).normalized;
        transform.position += direction * 10f;
        isSetupComeBacking = true;
    }

    void SetupComeBacking()
    {
        Vector3 direction = (DayPos - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        if (transform.position == DayPos) { isSetupComeBacking = false; DebuLog.C.AddDlList("SetupComeBacking end"); }
    }
<<<<<<< HEAD
=======

>>>>>>> d63f20ddf1c1ca6ddbac9f6def105e1c4ea72960
}
