using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        List<Sprite> eyeSps = EyeSet.C.SpritesList[eyeIndex];
        List<Sprite> legSps = LegSet.C.SpritesList[legIndex];
        Debug.Log(legIndex);
        SetNewShape(ballParson, ballSps, -30);
        SetNewShape(eyeParson, eyeSps, -20);
        SetNewShape(legParson, legSps, -10);
    }



    public void SetNewShape(GameObject parson, List<Sprite> sps, float z)
    {
        foreach (Transform child in parson.transform)
        {
            if (child != parson.transform) // 自身を除いて子オブジェクトを破壊
            {
                Destroy(child.gameObject);
            }
        }
        RectTransform parentRect = GetComponent<RectTransform>();
        int offset = 4;
        foreach (Sprite sp in sps)
        {
            Vector3 v = new Vector3(parentRect.anchoredPosition.x, parentRect.anchoredPosition.y, z - offset);
            GameObject newChild = Instantiate(shapeChildPrefab, v, Quaternion.identity, parson.transform);
            newChild.GetComponent<SpriteRenderer>().sprite = sp;
            offset += -1;
        }
    }
}
