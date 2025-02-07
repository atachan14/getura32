using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class TabManager : MonoBehaviour
{
    public static TabManager L;
    [SerializeField] GameObject shape;
    public GameObject SelectTab { get; set; }
    public string SelectParts { get; set; }

    private void Awake()
    {
        L = this;
    }
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickShape()
    {
        SelectTab = shape;
        DisplaySelectTab();
    }
    public void OnClickColor0()
    {
        SelectTab = OpenShapeDisplay.L.colorDisplayList[0].gameObject;
        DisplaySelectTab();
    }
    public void OnClickColor1()
    {
        SelectTab = OpenShapeDisplay.L.colorDisplayList[1].gameObject;
        DisplaySelectTab();
    }
    public void OnClickColor2()
    {
        SelectTab = OpenShapeDisplay.L.colorDisplayList[2].gameObject;
        DisplaySelectTab();
    }
    public void OnClickColor3()
    {
        SelectTab = OpenShapeDisplay.L.colorDisplayList[3].gameObject;
        DisplaySelectTab();
    }

    public void DisplaySelectTab()
    {
        Debug.Log($"TabManager dst {OpenShapeDisplay.L.colorDisplayList.Count}");
        foreach (ColorDisplay cd in OpenShapeDisplay.L.colorDisplayList)
        {
            cd.gameObject.SetActive(false);
            shape.SetActive(false);
        }
        SelectTab.SetActive(true);
    }
}
