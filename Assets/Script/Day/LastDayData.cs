using System.Collections.Generic;
using UnityEngine;

public class LastDayData : MonoBehaviour
{
    public static LastDayData C;
    public List<(ulong p0, ulong p1, int tribute)> PairIdList { get; set; } = new();
    public Dictionary<ulong,Vector3> TuraaPosDict { get; set; } =new();

    private void Awake()
    {
        C = this;
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
