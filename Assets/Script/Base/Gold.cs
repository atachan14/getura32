using TMPro;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public static Gold C;
    [SerializeField] TextMeshProUGUI goldTMP;
    int value = 0;
    int targetValue = 0;
    public int Value
    {
        get { return value; }
        set
        {
            targetValue = value;
        }
    }

    void Awake()
    {
        C = this;
    }

    void Update()
    {
        if (value != targetValue)
        {
            if (targetValue > value) value += (targetValue - value) / 10;
            //if (targetValue - value > 5000) value += 555;
            //if (targetValue - value > 1000) value += 222;
            //if (targetValue - value > 100) value += 22;
            if (targetValue > value) value++;

            if (targetValue < value) value -= (targetValue - value) / 10;
            //if (targetValue - value < -5000) value -= 555;
            //if (targetValue - value < -1000) value -= 222;
            //if (targetValue - value < -100) value -= 22;
            if (targetValue < value) value--;

            goldTMP.text = $"{value}G";
        }
    }
}
