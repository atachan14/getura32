using System.Collections.Generic;
using UnityEngine;

public class LastDayData : MonoBehaviour
{
    public static LastDayData S;
    public List<(ulong p0, ulong p1, int tribute)> PairIdList { get; set; } = new();

    private void Awake()
    {
        S = this;
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
