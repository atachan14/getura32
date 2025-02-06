using TMPro;
using Unity.Netcode;
using UnityEngine;

public class MatchingStatus : NetworkBehaviour
{

    public static MatchingStatus C;
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

    private ulong? partnerId = null;
    public ulong? PartnerId
    {
        get => partnerId;
        set
        {
            partnerId = value;
            if (partnerId != null && NetworkManager.Singleton.ConnectedClients[(ulong)partnerId].PlayerObject != partnerTuraa)
            {
                partnerTuraa = NetworkManager.Singleton.ConnectedClients[(ulong)partnerId].PlayerObject.gameObject;
            }
            namePlate.ChangeColor();
        }
    }
    

    private bool isPink = false;
    public bool IsPink
    {
        get => isPink;
        set
        {
            isPink = value;
            namePlate.ChangeColor();
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
            namePlate.ChangeColor();
        }
    }
    public int PartnerTribute { get; set; }
    public bool IsP0 { get; set; }



    private void Awake()
    {

    }
    void Start()
    {
        if (IsOwner) C = this;
        namePlate = GetComponent<NamePlate>();
    }
    void Update()
    {

    }
}
