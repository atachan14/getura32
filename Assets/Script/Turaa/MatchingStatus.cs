using TMPro;
using Unity.Netcode;
using UnityEngine;

public class MatchingStatus : NetworkBehaviour
{

    public static MatchingStatus C;
    public bool IsAlive { get; set; } = true;
    public bool IsPink { get; set; } = false;
    public bool IsRed { get; set; } = false;
    public bool IsStick { get; set; } = false;
    public bool IsPlz { get; set; } = false;
    public bool IsCant { get; set; } = false;

    public GameObject PartnerTuraa { get; set; }
    public ulong? PartnerId { get; set; }
    public int PartnerTribute {  get; set; }
    public bool IsP0 { get; set; }



    private void Awake()
    {

    }
    void Start()
    {
        if (IsOwner) C = this;
    }
    void Update()
    {

    }
}
