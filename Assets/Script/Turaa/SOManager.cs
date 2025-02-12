using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SoManager : NetworkBehaviour
{
    MatchingStatus mStatus;
    int mySo = 0;

    void Start()
    {
        mStatus = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<MatchingStatus>();

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
        Debug.Log($"mySoSelect IsRed:{mStatus.IsRed}");
        if (mStatus.IsPlz) mySo = 1000;
        else if (mStatus.IsRed) mySo = 100;
        else if (mStatus.IsPink) mySo = 100;
        else if (mStatus.IsCant) mySo = -1;
        else mySo = 0;

        ChangeSo();

    }

    public void OtherSoSelect()
    {
        Debug.Log($"otherSoSelect RedTarget==go:{mStatus.RedTarget == gameObject}");
        if (mStatus.RedTarget==gameObject) mySo = 100;
        else if(mStatus.PinkTargetList.Contains(gameObject)) mySo = 100;
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
