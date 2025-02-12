using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SoManager : NetworkBehaviour
{
    MatchingStatus ownerStatus;
    MatchingStatus myStatus;
    int mySo = 0;

    void Start()
    {
        ownerStatus = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<MatchingStatus>();
        myStatus = GetComponent<MatchingStatus>();

    }

    // Update is called once per frame
    //void Update()
    
    public void SoSelect()
    {
        if (IsOwner) MySoSelect();
        else OtherSoSelect();
    }
    
    public void MySoSelect()
    {
        Debug.Log($"mySoSelect IsRed:{ownerStatus.IsRed}");
        if (ownerStatus.IsPlz) mySo = 1000;
        else if (ownerStatus.IsRed) mySo = 100;
        else if (ownerStatus.IsPink) mySo = 100;
        else if (ownerStatus.IsCant) mySo = -1;
        else mySo = 0;

        ChangeSo();

    }

    public void OtherSoSelect()
    {
        Debug.Log($"otherSoSelect RedTarget==go:{ownerStatus.RedTarget == gameObject}");
        if (myStatus.IsPlz) mySo = 1000;
        else if (ownerStatus.RedTarget==gameObject) mySo = 100;
        else if(ownerStatus.PinkTargetList.Contains(gameObject)) mySo = 100;
        else mySo = 0;

        ChangeSo() ;
    }

    public void ChangeSo()
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer r in srs)
        {
            r.sortingOrder = mySo;
        }

       

    }
}
