using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class MatchingStatus : NetworkBehaviour
{

    public static MatchingStatus C;
    public NamePlate namePlate;
    private bool isAlive = true;
    public bool IsAlive
    {
        get => isAlive;
        set
        {
            isAlive = value;
            namePlate.ChangeColor();
        }
    }

    private bool isPlz = false;
    public bool IsPlz
    {
        get => isPlz;
        set
        {
            isPlz = value;
            PlzEffect.C.ChangeIsPlzServerRpc(value);
            namePlate.ChangeColor();
        }
    }

    private bool isCant = false;
    public bool IsCant
    {
        get => isCant;
        set
        {
            isCant = value;
            CantEffect.C.ChangeIsCantServerRpc(value);
            namePlate.ChangeColor();
        }
    }
    private ulong? redId = null;
    public ulong? RedId
    {
        get => redId;
        set
        {
            redId = value;
            RedTuraa = value != null
                ? NetworkManager.Singleton.ConnectedClients[(ulong)redId].PlayerObject.gameObject
                : null;
        }
    }

    private ulong? partnerId = null;
    public ulong? PartnerId
    {
        get => partnerId;
        set
        {
            partnerId = value;
            if (partnerId != null && NetworkManager.Singleton.ConnectedClients[(ulong)partnerId].PlayerObject != partnerTuraa)
            {
                PartnerTuraa = NetworkManager.Singleton.ConnectedClients[(ulong)partnerId].PlayerObject.gameObject;
            }
        }
    }
    private List<(GameObject turaa, int tribute)> pinkTupleLIst = new();
    public List<(GameObject turaa, int tribute)> PinkTupleList
    {
        get => pinkTupleLIst;
        set
        {
            pinkTupleLIst = value;
           
           
        }
    }




    private GameObject redTuraa = null;
    public GameObject RedTuraa
    {
        get => redTuraa;
        set
        {
            DebuLog.C.AddDlList("mStatus SetRedTuraa");
            redTuraa = value;
            redId = redTuraa ? GetComponent<NetworkObject>().OwnerClientId : null;
            DebuLog.C.AddDlList($"mStatus redId:{redId}");
            namePlate.ChangeColor();
            MatchingEffect.CI.RedEffect(value);
            TargetInfoManager.C.SelectType();
            TopInfo.C.MinusForRed(value);
        }

    }

    private GameObject partnerTuraa = null;
    public GameObject PartnerTuraa
    {
        get => partnerTuraa;
        set
        {
            partnerTuraa = value;
            if (partnerTuraa.TryGetComponent<NetworkObject>(out var networkObject) && networkObject.OwnerClientId != partnerId)
            {
                partnerId = networkObject.OwnerClientId;
            }
            
            if (partnerId == redId) redId = null;
            StickEffect.C.StickingServerRpc(PartnerTuraa);
            if(partnerTuraa) AgreeTribute = TopInfo.C.GetTributeFromId((ulong)partnerId);
            namePlate.ChangeColor();
            TargetInfoManager.C.SelectType();
        }
    }

   

    public int AgreeTribute { get; set; }

    private void Awake()
    {
       C = this;
    }

    void Start()
    {
       
        namePlate = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.GetComponent<NamePlate>();
        if(namePlate==null)Debug.Log($"namePlate isnot");
        if (namePlate != null) Debug.Log($"namePlate is");
    }

}
