using System.Collections.Generic;
using System.Linq;
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
            if (isAlive && !value) StartCoroutine(GetComponent<DieStager>().DieStaging());
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
            GetComponent<PlzEffect>().ExeServerRpc(value);
            namePlate.ChangeColor();
        }
    }

    private bool isCant = false;
    public bool IsCant
    {
        get => isCant;
        set
        {
            Debug.Log("Set_IsCant");
            isCant = value;
            if (value) IsCantReset();
            GetComponent<CantEffect>().ExeServerRpc(value);
            namePlate.ChangeColor();
        }
    }

    private GameObject redTarget;
    public GameObject RedTarget
    {
        get { return redTarget; }
        set
        {
            if (redTarget != null) MatchingEffect.CI.OffRedEffect(redTarget);
            if (value != null) MatchingEffect.CI.OnRedEffect(value);

            redTarget = value;
            namePlate.ChangeColor();
        }
    }

    public bool IsRed
    {
        get { return redTarget != null; }
    }


    ulong partnerId = 9999;
    public ulong PartnerId
    {
        get { return partnerId; }
        set
        {
            partnerId = value;
            if (value != 9999) PartnerTuraa = NetworkManager.Singleton.ConnectedClients[value].PlayerObject.gameObject;
            else partnerTuraa = null;
        }
    }

    public bool HasPartner
    {
        get { return partnerId != 9999; }
    }

    private List<(GameObject senderTuraa, int money)> pinkList = new();
    public List<(GameObject senderTuraa, int money)> PinkList
    {
        get { return pinkList; }
        set
        {
            pinkList = value;
            if (IsOwner) LoveCallsManage.C.ShowLovePopups();
        }
    }
    public bool IsPink
    {
        get { return pinkList.Count > 0; }
    }

    public List<GameObject> PinkTargetList
    {
        get
        {
            return pinkList.Select(item => item.senderTuraa).ToList();
        }
    }


    GameObject partnerTuraa;
    public GameObject PartnerTuraa
    {
        get { return partnerTuraa; }
        set
        {
            partnerTuraa = value;
            if (value != null) partnerId = value.GetComponent<NetworkObject>().OwnerClientId;
            else partnerId = 9999;
            GetComponent<StickEffect>().StickingServerRpc((value != null));
            namePlate.ChangeColor();
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

    public void Reset()
    {
        IsPlz = false;
        IsCant = false;
        RedTarget = null;
        PartnerId = 9999;
        PinkList = new();
        PartnerTribute = 0;
    }

    void IsCantReset()
    {

    }

}
