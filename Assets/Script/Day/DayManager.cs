using TMPro;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class DayManager : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI roomSizeTMP;
    [SerializeField] TextMeshProUGUI aliveSizeTMP;
    [SerializeField] TextMeshProUGUI timeTMP;

    private int remainingTime;

    void Start()
    {
        if (IsHost)
        {
            roomSizeTMP.text = RoomSetting.CI.RoomSize.ToString();
            timeTMP.text = RoomSetting.CI.TimeSize.ToString();
            remainingTime = RoomSetting.CI.TimeSize;

            StartCoroutine(TimerCoroutine());
        }
    }

    private System.Collections.IEnumerator TimerCoroutine()
    {
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;
            timeTMP.text = remainingTime.ToString();
        }

        TimeUpClientRpc();
        
    }
    [ClientRpc]
    public void TimeUpClientRpc()
    {
        DebugWndow.CI.AddDlList("timeup");
    }


    void Update()
    {
        if (IsHost)
        {
            string rs = roomSizeTMP.text;
            string t = timeTMP.text;
            UpdateClientRpc(rs, t);
        }
    }
    [ClientRpc]
    public void UpdateClientRpc(string rs, string t)
    {
        roomSizeTMP.text = rs;
        timeTMP.text = t;
    }
}
