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
    }
    public void NightStaging()
    {
        DebuLog.C.AddDlList($"NightStart:{transform.position}");
        CameraController.C.NightCamera();
        InNight();
        PairVisible();
        StartCoroutine(OnLeftWalking());
        DebuLog.C.AddDlList($"StartCoroutine OnLeftWalking");
    }
   
    void InNight()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            client.Value.PlayerObject.gameObject.SetActive(false);
            client.Value.PlayerObject.gameObject.GetComponent<NetworkTransform>().enabled = false;
            client.Value.PlayerObject.gameObject.GetComponent<Rigidbody2D>().simulated = false;
        }
    }

    void PairVisible()
    {
        OneVisible(myTuraa, 0f);
        OneVisible(MatchingStatus.C.PartnerTuraa, 1.3f);

        DebuLog.C.AddDlList($"pairVisible end");
    }

    void OneVisible(GameObject turaa, float offset)
    {
        turaa.SetActive(true);
        turaa.transform.position = new Vector3(1015f + offset, 995f, -11);
        //turaa.GetComponent<Rigidbody2D>().simulated = true;
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
        }
    }

    void IntoWalk()
    {
        IntoInvisible(mySps);
        IntoInvisible(partnerSps);

        IntoInvisibleTMP(myTMP);
        IntoInvisibleTMP(partnerTMP);

        IntoRightUp(myTuraa);
        IntoRightUp(MatchingStatus.C.PartnerTuraa);
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
        if (sps[0].color.a == 0) DebuLog.C.AddDlList($"intoInvisi end");
    }

    void IntoInvisibleTMP(TextMeshProUGUI[] tmps)
    {
        foreach (TextMeshProUGUI tmp in tmps)
        {
            Color color = tmp.color;
            color.a -= intoInvisibleSpeed * Time.deltaTime;
            if (color.a < 0) { color.a = 0; isIntoWalking = false; }
            tmp.color = color;
        }
        if (tmps[0].color.a == 0) IntoHotel();
    }

    void IntoRightUp(GameObject turaa)
    {
        turaa.transform.position += intoRightUpSpeed * Time.deltaTime; ;
    }

    void IntoHotel()
    {
        isIntoWalking = false ;
        NightCalcer.C.NightCalcStart();
    }



}
