using Unity.Netcode;
using UnityEngine;

public class MatchingStatus : NetworkBehaviour
{
    public static MatchingStatus C;
    public bool IsPink { get; set; }
    public bool IsRed { get; set; }
    public bool IsStick { get; set; }
    public bool IsPlz { get; set; }
    public bool IsCant { get; set; }

    public GameObject PartnerTuraa { get; set; }
    public ulong? PartnerId { get; set; }
    public bool IsP0 {  get; set; }

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
