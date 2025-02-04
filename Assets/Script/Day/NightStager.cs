using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class NightStager : NetworkBehaviour
{
    public static NightStager C;
    GameObject myTuraa;
    SpriteRenderer[] mySps;
    SpriteRenderer[] partnerSps;
    TextMeshProUGUI[] myTMP;
    TextMeshProUGUI[] partnerTMP;

    bool isLeftWalking = false;
    bool isIntoWalking = false;
    int hotelCount = 0;

    Vector3 leftWalkSpeed = new(-4f, 0, 0);
    float intoInvisibleSpeed = 0.5f;
    Vector3 intoRightUpSpeed = new(1f, 1f, 0);

    private void Awake()
    {
        C = this;
    }

    void Start()
    {
        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        mySps = myTuraa.GetComponentsInChildren<SpriteRenderer>();
        myTMP = myTuraa.GetComponentsInChildren<TextMeshProUGUI>();
        //partnerSps = MatchingStatus.C.PartnerTuraa.GetComponentsInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        if (isLeftWalking) LeftWalk();
        if (isIntoWalking) IntoWalk();
        if (hotelCount == 2) EndStaging();
    }
    public void NightStart()
    {
        TestServerRpc(NetworkManager.Singleton.LocalClientId, 0);
        PairVisible();
        StartCoroutine(OnLeftWalking());
        DebuLog.C.AddDlList($"1  IsSpawned: {IsSpawned}, IsOwner: {IsOwner}, IsServer: {IsServer}, IsClient: {IsClient}");
        TestServerRpc(NetworkManager.Singleton.LocalClientId, 1);
    }

    void PairVisible()
    {
        OneVisible(myTuraa, 0f);
        OneVisible(MatchingStatus.C.PartnerTuraa, 1.3f);
    }

    void OneVisible(GameObject turaa, float offset)
    {
        turaa.SetActive(true);
        turaa.GetComponent<NetworkTransform>().enabled = false;
        turaa.GetComponent<Rigidbody2D>().simulated = false;
        turaa.transform.position = new Vector3(1015f + offset, 995f, -11);
    }

    IEnumerator OnLeftWalking()
    {
        yield return new WaitForSeconds(1f);
        isLeftWalking = true;
    }

    void LeftWalk()
    {
        myTuraa.transform.position += leftWalkSpeed * Time.deltaTime;
        MatchingStatus.C.PartnerTuraa.transform.position += leftWalkSpeed * Time.deltaTime;
        if (myTuraa.transform.position.x < 998f)
        {
            isLeftWalking = false;
            isIntoWalking = true;

            partnerSps = MatchingStatus.C.PartnerTuraa.GetComponentsInChildren<SpriteRenderer>();
            partnerTMP = MatchingStatus.C.PartnerTuraa.GetComponentsInChildren<TextMeshProUGUI>();
            TestServerRpc(NetworkManager.Singleton.LocalClientId, 2);
            DebuLog.C.AddDlList($"2  IsSpawned: {IsSpawned}, IsOwner: {IsOwner}, IsServer: {IsServer}, IsClient: {IsClient}");
        }
    }

    void IntoWalk()
    {
        //TestServerRpc(NetworkManager.Singleton.LocalClientId, 2.1f);
        IntoInvisible(mySps);
        IntoInvisible(partnerSps);

        IntoInvisibleTMP(myTMP);
        IntoInvisibleTMP(partnerTMP);

        IntoRightUp(myTuraa);
        IntoRightUp(MatchingStatus.C.PartnerTuraa);
        //TestServerRpc(NetworkManager.Singleton.LocalClientId, 2.3f);
    }

    void IntoInvisible(SpriteRenderer[] sps)
    {
        foreach (SpriteRenderer sp in sps)
        {
            Color color = sp.color;
            color.a -= intoInvisibleSpeed * Time.deltaTime;
            if (color.a < 0) { color.a = 0; isIntoWalking = false; }
            sp.color = color;
        }
    }

    void IntoInvisibleTMP(TextMeshProUGUI[] tmps)
    {
        //TestServerRpc(NetworkManager.Singleton.LocalClientId, 2.21f);
        foreach (TextMeshProUGUI tmp in tmps)
        {
            Color color = tmp.color;
            color.a -= intoInvisibleSpeed * Time.deltaTime;
           
            if (color.a < 0) 
            { 
                color.a = 0; 
                isIntoWalking = false; 
            }
            tmp.color = color;
        }
        //DebuLog.C.AddDlList($"2.22  IsSpawned: {IsSpawned}, IsOwner: {IsOwner}, IsServer: {IsServer}, IsClient: {IsClient}");
        //TestServerRpc(NetworkManager.Singleton.LocalClientId, 2.22f);
        if (!isIntoWalking) IntoHotel();
    }

    void IntoRightUp(GameObject turaa)
    {
        turaa.transform.position += intoRightUpSpeed * Time.deltaTime; ;
    }

    void IntoHotel()
    {
        hotelCount++;
        DebuLog.C.AddDlList($"3  IsSpawned: {IsSpawned}, IsOwner: {IsOwner}, IsServer: {IsServer}, IsClient: {IsClient}");
        TestServerRpc(NetworkManager.Singleton.LocalClientId, 3);
    }
    void EndStaging()
    {
        TestServerRpc(NetworkManager.Singleton.LocalClientId, 4);
        hotelCount = 0;
        isIntoWalking = false;
        DebuLog.C.AddDlList($"5  IsSpawned: {IsSpawned}, IsOwner: {IsOwner}, IsServer: {IsServer}, IsClient: {IsClient}");
        TestServerRpc(NetworkManager.Singleton.LocalClientId, 5);
        NightCalcer.C.StartCalc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void TestServerRpc(ulong id, float f)
    {
        DebuLog.C.AddDlList($"test night index : {f} , id : {id}");
    }
}
