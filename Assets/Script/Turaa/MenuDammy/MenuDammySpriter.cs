using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MenuDammySpriter : MonoBehaviour
{
    [SerializeField] GameObject Ball;
    [SerializeField] GameObject Eye;
    [SerializeField] GameObject Leg;
    [SerializeField] GameObject partChildPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeShape(string selectPart , List<Sprite> sps)
    {
        switch (selectPart)
        {
            case "Ball":
                SetNewShape(Ball,sps);
                return;
            case "Eye":
                SetNewShape(Eye,sps);
                return;
            case "Leg":
                SetNewShape (Leg,sps);
                return ;
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

    public void ChangeColor(int select,string color,int value)
    {

    }
}
