using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class MatchingStatus : NetworkBehaviour
{

    public static MatchingStatus C { get; set; }
    NamePlate namePlate;
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
            namePlate.ChangeColor();
        }
    }
    private bool isRed = false;
    public bool IsRed
    {
        get => isRed;
        set
        {
            isRed = value;
            namePlate.ChangeColor();
        }
    }

    ulong partnerId = 9999;
    public ulong PartnerId
    {
        get { return partnerId; }
        set
        {
            partnerId = value;
            if (value != 9999) PartnerTuraa = NetworkManager.Singleton.ConnectedClients[value].PlayerObject.gameObject;
        }
    }
    private List<(GameObject senderTuraa, int money)> pinkList = new ();
    public List<(GameObject senderTuraa, int money)> PinkList { get { return pinkList; } }


    GameObject partnerTuraa;
    public GameObject PartnerTuraa
    {
        get { return partnerTuraa; }
        set
        {
            partnerTuraa = value;
            if(value != null) partnerId = value.GetComponent<NetworkObject>().OwnerClientId;

        }
    }




    public int PartnerTribute { get; set; }


    private void Awake()
    {
       
    }

    private void Start()
    {
        if (IsOwner) C = this;
        namePlate = GetComponent<NamePlate>();
    }
}
