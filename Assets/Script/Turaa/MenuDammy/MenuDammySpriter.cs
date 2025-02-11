using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Drawing;
using Color = UnityEngine.Color;

public class MenuDammySpriter : MonoBehaviour
{
    [SerializeField] GameObject Ball;
    [SerializeField] GameObject Eye;
    [SerializeField] GameObject Leg;
    [SerializeField] GameObject partChildPrefab;
  

    public void ChangeShape(string selectPart, List<Sprite> sps)
    {
        switch (selectPart)
        {
            case "Ball":
                SetNewShape(Ball, sps);
                return;
            case "Eye":
                SetNewShape(Eye, sps);
                return;
            case "Leg":
                SetNewShape(Leg, sps);
                return;
        }
    }

    public void SetNewShape(GameObject parson, List<Sprite> sps)
    {
        foreach (Transform child in parson.transform)
        {
            if (child != parson.transform) // 自身を除いて子オブジェクトを破壊
            {
                Destroy(child.gameObject);
            }
        }
        foreach (Sprite sp in sps)
        {
            GameObject newChild = Instantiate(partChildPrefab, parson.transform);
            newChild.GetComponent<SpriteRenderer>().sprite = sp;
        }
    }

    public void ChangeColor(string part, int index)
    {
        switch (part)
        {
            case "Ball":
                SpriteRenderer[] ballSrs = Ball.GetComponentsInChildren<SpriteRenderer>();
                if (ballSrs[index] != null) ballSrs[index].color = GetColorFromPrefs(part, index);
                return;
            case "Eye":
                SpriteRenderer[] eyeSrs = Eye.GetComponentsInChildren<SpriteRenderer>();
                if (eyeSrs[index] != null) eyeSrs[index].color = GetColorFromPrefs(part, index);
                return;
            case "Leg":
                SpriteRenderer[] legSrs = Leg.GetComponentsInChildren<SpriteRenderer>();
                if (legSrs[index] != null) legSrs[index].color = GetColorFromPrefs(part, index);
                return;
        }
    }


    public Color GetColorFromPrefs(string part, int index)
    {
        string savedColor = PlayerPrefs.GetString($"{part}Color{index}", "1,1,1,1");
        string[] rgba = savedColor.Split(',');
        Color c = new
        (
            float.Parse(rgba[0]),
            float.Parse(rgba[1]),
            float.Parse(rgba[2]),
            float.Parse(rgba[3])
        );
        return c;
    }
}
