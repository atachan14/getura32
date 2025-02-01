using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class NightManager : NetworkBehaviour
{
    public static NightManager C;
    GameObject myTuraa;
    SpriteRenderer[] mySps;
    SpriteRenderer[] partnerSps;

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
        //partnerSps = MatchingStatus.C.PartnerTuraa.GetComponentsInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        if (isLeftWalking) LeftWalk();
        if (isIntoWalking) IntoWalk();
    }
    public void NightStart()
    {
        DebuLog.C.AddDlList($"NightStart:{transform.position}");
        CameraController.C.NightCamera();
        OtherPartnerInvisible();
        PairVisible();
        StartCoroutine(OnLeftWalking());
        DebuLog.C.AddDlList($"StartCoroutine OnLeftWalking");
    }
    void OtherPartnerInvisible()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            client.Value.PlayerObject.gameObject.SetActive(false);
        }
    }

    void PairVisible()
    {
        
        OneVisible(myTuraa, 0f);
        OneVisible(MatchingStatus.C.PartnerTuraa, 1f);

        DebuLog.C.AddDlList($"pairVisible end");
    }

    void OneVisible(GameObject turaa, float offset)
    {
        turaa.SetActive(true);
        turaa.GetComponent<NetworkTransform>().enabled = false;
        turaa.GetComponent<Rigidbody2D>().simulated = false;
        turaa.transform.position = new Vector3(1020f + offset, 995f, -11);
        //turaa.GetComponent<Rigidbody2D>().simulated = true;
    }

    IEnumerator OnLeftWalking()
    {
        yield return new WaitForSeconds(2f);
        isLeftWalking = true;
        DebuLog.C.AddDlList($"isLeftWalking:{isLeftWalking}");
        DebuLog.C.AddDlList($"IntoWalk() called on {NetworkManager.Singleton.LocalClientId}");
    }

    void LeftWalk()
    {
        myTuraa.transform.position += leftWalkSpeed * Time.deltaTime;
        MatchingStatus.C.PartnerTuraa.transform.position += leftWalkSpeed * Time.deltaTime;
        if (myTuraa.transform.position.x < 998f)
        {
            isLeftWalking = false;
            isIntoWalking = true;
            DebuLog.C.AddDlList($"isIntoWalking=true");
            partnerSps = MatchingStatus.C.PartnerTuraa.GetComponentsInChildren<SpriteRenderer>();
            DebuLog.C.AddDlList($"partnerSps.Length:{partnerSps.Length}");
        }
    }

    void IntoWalk()
    {
        IntoInvisible(mySps);
        IntoInvisible(partnerSps);
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

    void IntoRightUp(GameObject turaa)
    {
        turaa.transform.position += intoRightUpSpeed * Time.deltaTime; ;
    }


}
