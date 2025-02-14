using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Charm : MonoBehaviour
{
    public static Charm C;
    [SerializeField] TextMeshProUGUI charmTMP;
    int value = 0;
    int targetValue = 0;
    public int Value
    {
        get { return value; }
        set
        {
            targetValue = value;
            DDOLReceiver.C.ReportCharm(value);
        }
    }

    void Awake()
    {
        C = this;
    }

    private void Start()
    {
    }

    void Update()
    {
        if (value != targetValue)
        {
            if (value < targetValue) value += (targetValue - value) / 10;
            if (value < targetValue) value++;
            if (value > targetValue) value -= (targetValue - value) / 10;
            if (value > targetValue) value--;

            charmTMP.text = $"Charm: {value}";
        }
    }

   
}
