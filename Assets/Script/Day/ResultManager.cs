using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public static ResultManager S;
    public List<ulong> rankList;

    private void Awake()
    {
        S = this;
    }
    

}
