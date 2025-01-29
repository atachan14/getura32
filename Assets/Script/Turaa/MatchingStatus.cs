using UnityEngine;

public class MatchingStatus : MonoBehaviour
{
    public bool IsPink { get; set; }
    public bool IsRed { get; set; }
    public bool IsStick { get; set; }
    public bool IsPlz { get; set; }
    public bool IsCant { get; set; }

    public GameObject PartnerTuraa { get; set; }
    public ulong? PartnerId { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
