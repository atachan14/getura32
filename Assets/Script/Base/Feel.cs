using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Feel : MonoBehaviour
{
    public static Feel C;
    [SerializeField] TextMeshProUGUI feelTMP;
    int value = 100;
    int targetValue = 100;
    MatchingStatus mStatus;
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
        if (mStatus == null && NetworkManager.Singleton != null && NetworkManager.Singleton.LocalClient.PlayerObject != null)
        {
            mStatus = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<MatchingStatus>();
        }
        if (value != targetValue)
        {
            if (value < targetValue) value += (targetValue - value) / 10;
            if (value < targetValue) value++;
            if (value > targetValue) value -= (value - targetValue) / 10;
            if (value > targetValue) value--;

            feelTMP.text = $"Feel: {value}";
        }

        if (mStatus.IsAlive && value <= 0)
        {
            mStatus.IsAlive = false;
        }
    }

    public IEnumerator AloneFeel()
    {
        float time = 0;
        float duration = 5;
        while (time < duration)
        {
            time += Time.deltaTime;
            Value = Mathf.RoundToInt(Mathf.Lerp(value, 0, time / duration));
            yield return null;
        }

    }
}
