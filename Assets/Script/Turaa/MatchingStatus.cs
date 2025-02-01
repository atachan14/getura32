using TMPro;
using Unity.Netcode;
using UnityEngine;

public class MatchingStatus : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI lpTMP;
    public static MatchingStatus C;
    public bool IsPink { get; set; }
    public bool IsRed { get; set; }
    public bool IsStick { get; set; }
    public bool IsPlz { get; set; }
    public bool IsCant { get; set; }

    public GameObject PartnerTuraa { get; set; }
    public ulong? PartnerId { get; set; }
    public bool IsP0 {  get; set; }

    public int[] myLPs { get; set; }
    private int displayLp;
    public int DisplayLp
    {
        get { return displayLp; }
        set
        {
            displayLp = value;  
            lpTMP.text = value.ToString();
            float t = Mathf.InverseLerp(0, 100, displayLp); 
            lpTMP.color = Color.Lerp(Color.blue, Color.magenta, t);
        }
    }

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
