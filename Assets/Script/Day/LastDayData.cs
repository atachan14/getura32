using System.Collections.Generic;
using UnityEngine;

public class LastDayData : MonoBehaviour
{
    public static LastDayData C;
    public List<(ulong p0, ulong p1, int tribute)> PairIdList { get; set; } = new();
    public List<ulong> AlonerList { get; set; } = new();

    private void Awake()
    {
        C = this;
    }

    public void ResetData()
    {
        PairIdList.Clear();
        AlonerList.Clear();
    }

}
