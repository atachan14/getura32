using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LoveCallWindowManager : MonoBehaviour
{
    private List<(ulong senderId, int money)> loveCallList = new List<(ulong senderId, int money)>();
    private int xDistance = 40;
    private int yDistance = 15;
    private int maxYcount = 5;
    public GameObject LovePopup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ReceiveLoveCall(ulong senderId, int money)
    {
        loveCallList.Add((senderId, money));
        ShowLovePopups();
    }

    void ShowLovePopups()
    {
        for (int i = 0; i < loveCallList.Count; i++)
        {
            // 左端から縦に表示して、適当な数で列移動
            Vector3 point = new(i / maxYcount * xDistance, -(i % maxYcount) * yDistance, 0);
            GameObject popup = Instantiate(LovePopup, point, Quaternion.identity, this.transform);

            var popupUI = popup.GetComponent<LovePopupManager>();
            if (popupUI != null)
            {
                popupUI.SetData(loveCallList[i].senderId, loveCallList[i].money);
            }
        }
    }
}
