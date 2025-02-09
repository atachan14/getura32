using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Feel : MonoBehaviour
{
    public static Feel C;
    [SerializeField] TextMeshProUGUI feelTMP;
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
            if (value < targetValue) value += (targetValue - value) / 10;
            if (value < targetValue) value++;
            if (value > targetValue) value -= (value - targetValue) / 10;
            if (value > targetValue) value--;

            feelTMP.text = $"Feel: {value}";
        }
    }

    public IEnumerator AloneFeel()
    {
            float time = 0;
        float duration = 8;
            while (time < duration)
            {
                time += Time.deltaTime;
                Value = Mathf.RoundToInt(Mathf.Lerp(value, 0, time / duration));
                yield return null;
            }
    }
}
