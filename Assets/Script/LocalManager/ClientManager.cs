using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ClientManager : NetworkBehaviour
{
    public static ClientManager CI;

    [SerializeField] private MatchingEffect mEffect;
    [SerializeField] private MatchingStatus mStatus;
    [SerializeField] private LoveCallsManage loveCalls;

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

    public void haveLoveCalls(List<GameObject> senderTuraaList)
    {
        mEffect.OnPinkEffect();
    }

    public void dontHaveLoveCalls()
    {
        mEffect.OnPinkEffect();
    }

    public void SendLoveCall(ulong targetId)
    {
        GameObject targetTuraa = NetworkManager.Singleton.ConnectedClients[targetId].PlayerObject.gameObject;
        mEffect.OnRedEffect(targetTuraa);
    }

    public void SendLoveCallCansell()
    {
        mEffect.OffRedEffect();
    }

    public void ReceiveLoveCall(GameObject senderTuraa, int money)
    {
        loveCalls.AddLoveCallList(senderTuraa, money);

        
    }

    public void ReceiveLoveCallCansell(ulong senderId)
    {

    }
    public void ReceiveOK()
    {
        mEffect.OffRedEffect();
    }

    public void ReceiveNG()
    {
        mEffect.OffRedEffect();
    }
}
