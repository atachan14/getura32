using Unity.Netcode;
using UnityEngine;

public class TimeUpLeave : NetworkBehaviour
{
    [SerializeField] float speed = 7f;
    Vector3 leaveDirection;
    bool IsP0Leaving = false;

    GameObject p0;
    bool IsP1Leaving = false;
    public Vector3 DayPos { get; set; }
    //bool isComeBackingToDay = false;

    //public NetworkVariable<bool> isDay = new(true);
    //public bool IsDay
    //{
    //    get => isDay.Value;
    //    set => isDay.Value = value;
    //}

    void Update()
    {
        if (IsP0Leaving) P0Leave();
        if (IsP1Leaving) P1Leave();
        //if (isComeBackingToDay) ComeBackingToDay();
    }

    public void OnP0Leave(Vector3 direction)
    {
        this.leaveDirection = direction;
        IsP0Leaving = true;
        //IsDay = false;
    }
    public void OnP1Leave(GameObject p0)
    {
        this.p0 = p0;
        IsP1Leaving = true;
        //IsDay = false;
    }

    void P0Leave()
    {
        transform.position += speed * Time.deltaTime * leaveDirection;

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

    public void ComeBackToDayFlow()
    {
        transform.position = DayPos;
        //Vector3 direction = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f).normalized;
        //transform.position = DayPos + direction * 15f;
        //isComeBackingToDay = true;
    }

    //void ComeBackingToDay()
    //{
    //    Vector3 direction = (DayPos - transform.position).normalized;
    //    transform.position += speed * Time.deltaTime * direction * 0.5f;
    //    if ((transform.position - DayPos).magnitude < 2f)
    //    {
    //        isComeBackingToDay = false;
    //        DebuLog.C.AddDlList("SetupComeBacking end");
    //    }
    //    transform.position= DayPos;
    //}
}
