using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;
public class OpenShapeDisplay : MonoBehaviour
{
    [SerializeField] MenuDammySpriter menuDammySpriter;
    [SerializeField] GameObject customUI;
    [SerializeField] GameObject shapeDisplay;
    [SerializeField] GameObject thumbnailPrefab;
    [SerializeField] GameObject thmChild;
    List<GameObject> thmbnailList;
    string selectPart;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckCollider();
        }
    }

    void CheckCollider()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
                if (hit.collider.CompareTag("Ball"))
                {
                    Debug.Log("ball");
                    BallOpen();
                }
                else if (hit.collider.CompareTag("Eye"))
                {
                    Debug.Log("Eye");
                    EyeOpen();
                }
                else if (hit.collider.CompareTag("Leg"))
                {
                    Debug.Log("Leg");
                }
        }
    }
    void BallOpen()
    {
        selectPart = "Ball";

        List<List<Sprite>> spsList = BallSet.C.SpritesList;
        GenerateShapeDisplay(spsList);
    }

    void EyeOpen()
    {
        selectPart = "Eye";
        List<List<Sprite>> spsList = EyeSet.C.SpritesList;
        GenerateShapeDisplay(spsList);
    }

    void GenerateShapeDisplay(List<List<Sprite>> spsList)
    {
        customUI.SetActive(true);
        ResetShapeDisplay();
        thmbnailList = GenerateThmbnailList(spsList);
        SetOnClickIndex();
        LineUpThmbnailList();
    }

    void ResetShapeDisplay()
    {
        foreach (Transform child in shapeDisplay.transform)
        {
            if (child != shapeDisplay.transform) // 自身を除いて子オブジェクトを破壊
            {
                Destroy(child.gameObject);
            }
        }
    }

    List<GameObject> GenerateThmbnailList(List<List<Sprite>> spsList)
    {
        List<GameObject> thmbnailList = new();

        foreach (List<Sprite> sps in spsList)
        {
            GameObject thm = Instantiate(thumbnailPrefab);
            thm.transform.SetParent(shapeDisplay.transform,false);
            List<GameObject> thmChildren = GenerateThmChildren(sps);
            foreach (GameObject child in thmChildren) child.transform.SetParent(thm.transform,false);
            thmbnailList.Add(thm);
        }
        return thmbnailList;
    }
    List<GameObject> GenerateThmChildren(List<Sprite> sps)
    {
        List<GameObject> thmChildren = new();
        foreach (Sprite sp in sps)
        {
            GameObject tc = Instantiate(thmChild);
            tc.AddComponent<Image>();
            tc.GetComponent<Image>().sprite = sp;
            tc.GetComponent<Image>().SetNativeSize();
            ScaleChousei(tc);
            thmChildren.Insert(0,tc);
        }
        return thmChildren;
    }

    void ScaleChousei(GameObject tc)
    {
        Image img = tc.GetComponent<Image>();
        RectTransform imgRect = img.GetComponent<RectTransform>();

        // 親の枠内に収めるためのスケール計算
        float scaleX = 120f /imgRect.rect.width;
        float scaleY = 130f / imgRect.rect.height;
        float scale = Mathf.Min(scaleX, scaleY); // はみ出さないように小さい方の比率を適用

        imgRect.localScale = new Vector3(scale, scale, 1f);
    }
    void SetOnClickIndex()
    {
        for(int i = 0; i < thmbnailList.Count; i++)
        {
            int index = i;
            thmbnailList[index].GetComponent<Button>().onClick.AddListener(() => OnThumbnailClicked(index));
        }
    }

    void LineUpThmbnailList()
    {
        for (int i = 0; i < thmbnailList.Count; i++)
        {
            thmbnailList[i].transform.position += new Vector3(5+i%3*130, i/3*150, 0);
        }
    }

    public void OnThumbnailClicked(int i)
    {
        List<Image> imageList = new List<Image>(thmbnailList[i].GetComponentsInChildren<Image>());
        List<Sprite> spList = new();
        foreach (Image image in imageList)
        {
            spList.Add(image.sprite);
        }
        Debug.Log($"OnThumClick{i}");
        menuDammySpriter.ChangeShape(selectPart, spList);
        PlayerPrefs.SetInt(selectPart, i);
    }
}
