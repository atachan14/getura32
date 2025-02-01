using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MatchingStatus : NetworkBehaviour
{
    
    public static MatchingStatus C;
    public bool IsRed { get; set; } = false;
    public bool IsPlz { get; set; } = false;
    public bool IsCant { get; set; } = false;
    public bool IsStick { get; set; } = false;
    public bool IsPink { get; set; }=false;
    

    public GameObject PartnerTuraa { get; set; }
    public ulong? PartnerId { get; set; }
    public bool IsP0 {  get; set; }


    private void Awake()
    {
        if (IsOwner) C = this;
    }



    void Start()
    {
       
    }
   


}
