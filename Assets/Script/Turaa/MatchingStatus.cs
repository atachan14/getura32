using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MatchingStatus : NetworkBehaviour
{

    public static MatchingStatus C { get; set; }
    NamePlate namePlate;
    TargetInfoManager targetIM;
    SoManager soManager;

    bool isNight;
    public bool IsNight
    {
        get { return isNight; }
        set
        {
            isNight = value;
            SetIsNightServerRpc(value);
        }
    }
    [ServerRpc]
    void SetIsNightServerRpc(bool b)
    {
        SetIsNightClientRpc(b);
    }

    [ClientRpc]
    void SetIsNightClientRpc(bool b)
    {
        isNight=b;
    }


    private GameObject target;
    public GameObject Target
    {
        get { return target; }
        set
        {
            if (!IsOwner) return;
            if(target) target.GetComponent<NamePlate>().targetShadow.SetActive(false);

            target = value;
            if (!targetIM) targetIM = TargetInfoManager.C;
            if (target) targetIM.gameObject.SetActive(true);
            if (target) targetIM.SetTarget(value);
            if (target) target.GetComponent<NamePlate>().targetShadow.SetActive(true);

            if (!target) targetIM.gameObject.SetActive(false);
        }
    }



    private bool isAlive = true;
    public bool IsAlive
    {
        get => isAlive;
        set
        {
            if (!IsOwner) return;
            if (isAlive && !value) StartCoroutine(GetComponent<DieStager>().DieStaging());
            isAlive = value;

            namePlate.ChangeColor();
        }
    }

    private  bool isPlz = false;
    public bool IsPlz
    {
        get => isPlz;
        set
        {
            if (!IsOwner) return;
            IsPlzServerRpc(value);
        }
    }
    [ServerRpc]
    public void IsPlzServerRpc(bool b)
    {
        IsPlzClientRpc(b);
    }

    [ClientRpc]
    void IsPlzClientRpc(bool b)
    {
        isPlz = b;
        GetComponent<PlzEffect>().ExePlz(b);
        namePlate.ChangeColor();
    }

    private bool isCant = false;
    public bool IsCant
    {
        get => isCant;
        set
        {
            if (!IsOwner) return;
            isCant = value;
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
            if (!IsOwner) return;
            redTarget = value;

            namePlate.ChangeColor();
            MatchingEffect.CI.RedEffect(value != null);
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
            if (!IsOwner) return;
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
            if (!IsOwner) return;
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
            if (!IsOwner) return;
            partnerTuraa = value;
            if (value != null) partnerId = value.GetComponent<NetworkObject>().OwnerClientId;
            else partnerId = 9999;
            GetComponent<StickEffect>().StickingServerRpc((value != null));

            namePlate.ChangeColor();
            if (value) namePlate.SetPartnerNameServerRpc(value.GetComponent<NamePlate>().GetName());
            else namePlate.SetPartnerNameServerRpc("");
        }
    }




    public int PartnerTribute { get; set; } = 0;


    private void Awake()
    {

    }

    private void Start()
    {
        if (IsOwner) C = this;
        namePlate = GetComponent<NamePlate>();
        soManager = GetComponent<SoManager>();

    }

    public void Reset()
    {
        Target = null;
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
