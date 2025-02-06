using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class TabManager : MonoBehaviour
{
    public static TabManager L;
    [SerializeField] GameObject shape;
    [SerializeField] GameObject colorTab0;
    [SerializeField] GameObject colorTab1;
    [SerializeField] GameObject colorTab2;
    [SerializeField] GameObject colorTab3;
    public GameObject[] colorTabs=new GameObject[3];

    public GameObject SelectTab { get; set; }
    public string SelectParts { get; set; }

    private void Awake()
    {
        L = this;
    }
    void Start()
    {
        colorTabs = new GameObject[] { colorTab0, colorTab1, colorTab2, colorTab3 };
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
        SelectTab = OpenShapeDisplay.L.ColorDisplayList[0].gameObject;
        DisplaySelectTab();
    }
    public void OnClickColor1()
    {
        SelectTab = OpenShapeDisplay.L.ColorDisplayList[1].gameObject;
        DisplaySelectTab();
    }
    public void OnClickColor2()
    {
        SelectTab = OpenShapeDisplay.L.ColorDisplayList[2].gameObject;
        DisplaySelectTab();
    }
    public void OnClickColor3()
    {
        SelectTab = OpenShapeDisplay.L.ColorDisplayList[3].gameObject;
        DisplaySelectTab();
    }

    public void DisplaySelectTab()
    {
        Debug.Log($"TabManager dst {OpenShapeDisplay.L.ColorDisplayList.Count}");
        foreach (ColorDisplay cd in OpenShapeDisplay.L.ColorDisplayList)
        {
            cd.gameObject.SetActive(false);
            shape.SetActive(false);
        }
        SelectTab.SetActive(true);
        Debug.Log($"dst SelectTab.SetActive:{SelectTab}");
    }
}
