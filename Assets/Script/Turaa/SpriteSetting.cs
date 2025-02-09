using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteSetting : NetworkBehaviour
{
    [SerializeField] GameObject ballParson;
    [SerializeField] GameObject eyeParson;
    [SerializeField] GameObject legParson;
    [SerializeField] GameObject shapeChildPrefab;



    public NetworkVariable<int> BallIndex { get; set; } = new();
    public NetworkVariable<int> EyeIndex { get; set; } = new();
    public NetworkVariable<int> LegIndex { get; set; } = new();

    int lastBall = 0;
    int lastEye = 0;
    int lastLeg = 0;

    void Start()
    {
        if (IsOwner)
        {
            SetIndexsServerRpc(PlayerPrefs.GetInt("Ball", 0), PlayerPrefs.GetInt("Eye", 0), PlayerPrefs.GetInt("Leg", 0));
        }
    }
    [ServerRpc]
    void SetIndexsServerRpc(int ball, int eye, int leg)
    {
        BallIndex.Value = ball;
        EyeIndex.Value = eye;
        LegIndex.Value = leg;
    }

    void Update()
    {
        if (BallIndex.Value != lastBall || EyeIndex.Value != lastEye || LegIndex.Value != lastLeg)
        {
            ChangeSprites(BallIndex.Value, EyeIndex.Value, LegIndex.Value);
            lastBall = BallIndex.Value;
            lastEye = EyeIndex.Value;
            lastLeg = LegIndex.Value;
        }
    }

    void ChangeSprites(int ballIndex, int eyeIndex, int legIndex)
    {
        List<Sprite> ballSps = BallSet.C.SpritesList[ballIndex];
        List<Sprite> EyeSps = EyeSet.C.SpritesList[eyeIndex];
        // List<Sprite> LegSps = EyeSet.C.SpritesList[legIndex];
        SetNewShape(ballParson, ballSps, 30);
        SetNewShape(eyeParson, EyeSps, 20);
        // SetNewShape(legParson, LegSps);
    }



    public void SetNewShape(GameObject parson, List<Sprite> sps, int so)
    {
        foreach (Transform child in parson.transform)
        {
            if (child != parson.transform) // ���g�������Ďq�I�u�W�F�N�g��j��
            {
                Destroy(child.gameObject);
            }
        }
        int offset = 4;
        foreach (Sprite sp in sps)
        {
            GameObject newChild = Instantiate(shapeChildPrefab, parson.transform, false);
            newChild.GetComponent<SpriteRenderer>().sprite = sp;
            newChild.GetComponent<SpriteRenderer>().sortingOrder += so + offset;
            offset += -1;
        }
    }
}
