using Unity.Netcode;
using UnityEngine;

public class NightManager : NetworkBehaviour
{
    public static NightManager C;
    GameObject myTuraa;
    SpriteRenderer[] mySps;
    SpriteRenderer[] partnerSps;

    bool isLeftWalking = false;
    bool isIntoWalking = false;

    Vector3 leftWalkSpeed = new Vector3(-0.01f, 0, 0);
    float intoInvisibleSpeed = 0.01f;
    Vector3 intoRightUpSpeed = new Vector3(0.1f, 0.1f, 0);

    private void Awake()
    {
        C= this;
    }

    void Start()
    {
        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        mySps = myTuraa.GetComponentsInChildren<SpriteRenderer>();
        partnerSps = MatchingStatus.C.PartnerTuraa.GetComponentsInChildren<SpriteRenderer>();

        
    }
    private void Update()
    {
        //if (isLeftWalking) LeftWalk();
        //if (isIntoWalking) IntoWalk();
    }
    public void NightStart()
    {
        DebuLog.C.AddDlList($"NightStart:{transform.position}");
        OtherPartnerInvisible();
        PairVisible();
        //isLeftWalking = true;
        DebuLog.C.AddDlList($"isLeftWalking=true");
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
        CameraController.C.NightCamera();
        OneVisible(myTuraa,-5f);
        OneVisible(MatchingStatus.C.PartnerTuraa,5f);
       
        DebuLog.C.AddDlList($"pairVisible end");
    }

    void OneVisible(GameObject turaa,float offset)
    {
        turaa.SetActive(true);
        turaa.GetComponent<Rigidbody2D>().simulated = false;
        turaa.transform.position = new Vector3(1020f, 995f, -11);
        turaa.GetComponent<Rigidbody2D>().simulated = true;
    }

    void LeftWalk()
    {
        myTuraa.transform.position += leftWalkSpeed * Time.deltaTime; ;
        MatchingStatus.C.PartnerTuraa.transform.position += leftWalkSpeed * Time.deltaTime; ;
        if (myTuraa.transform.position.x < 998f)
        {
            isLeftWalking = false;
            isIntoWalking = true;
            DebuLog.C.AddDlList($"isIntoWalking=true");
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
