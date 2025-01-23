using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MatchingManager : MonoBehaviour
{
    public static MatchingManager CI;

    [SerializeField] private MatchingEffect mEffect;
    [SerializeField] private MatchingStatus mStatus;
    [SerializeField] private LoveCallsManage loveCalls;
    [SerializeField] private TargetInfoManager targetInfo;

    private void Awake()
    {
        if (CI == null) CI = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void haveLoveCalls(List<GameObject> senderTuraaList)
    //{
    //    DebugWndow.CI.AddDlList("haveLoveCalls");
        
    //}

    //public void dontHaveLoveCalls()
    //{
        
    //}

    //public void SendLoveCall(ulong targetId)
    //{
    //    GameObject targetTuraa = NetworkManager.Singleton.ConnectedClients[targetId].PlayerObject.gameObject;
    //    mEffect.OnRedEffect(targetTuraa);
    //}

    //public void SendLoveCallCansell()
    //{
    //    mEffect.OffRedEffect();
    //}

    //public void ReceiveLoveCall(ulong senderId, int money)
    //{
    //    GameObject senderTuraa = NetworkManager.Singleton.ConnectedClients[senderId].PlayerObject.gameObject; 
    //    loveCalls.AddLoveCallList(senderTuraa, money);

        
    //}

    //public void ReceiveLoveCallCansell(ulong senderId)
    //{

    //}
    //public void ReceiveOK()
    //{
    //    mEffect.OffRedEffect();
    //}

    //public void ReceiveNG()
    //{
    //    mEffect.OffRedEffect();
    //}
}
