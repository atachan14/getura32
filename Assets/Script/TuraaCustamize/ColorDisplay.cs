using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorDisplay : MonoBehaviour
{
    public int Index { get; set; }
    public string part { get; set; }
    [SerializeField] TMP_InputField rField;
    [SerializeField] TMP_InputField bField;
    [SerializeField] TMP_InputField gField;
    [SerializeField] TMP_InputField aField;
    void Start()
    {

    }

    void Update()
    {

    }

    public void SetupIndex(string part ,int i)
    {
        Index = i;
        this.part = part; 
        MyColorToSetField();
    }

    void MyColorToSetField()
    {
        string savedColor = PlayerPrefs.GetString($"{part}Color{Index}", "1,1,1,1");
        string[] rgba = savedColor.Split(',');
        rField.text = rgba[0];
        bField.text = rgba[1];
        gField.text = rgba[2];
        aField.text = rgba[3];
    }


    public void OnSubmitColor()
    {
        Color myColor = new Color
        (
            float.Parse(rField.text),
            float.Parse(gField.text),
            float.Parse(bField.text),
            float.Parse(aField.text)
        );

        PlayerPrefs.SetString($"{part}Color{Index}", $"{myColor.r},{myColor.g},{myColor.b},{myColor.a}");
        // changeColor atode kaku
    }


    public void OnClickRed()
    {
        PlayerPrefs.SetString($"{part}Color{Index}", $"{1},{0},{0},{1}");
    }
    public void OnClickBlue()
    {
        PlayerPrefs.SetString($"{part}Color{Index}", $"{0},{1},{0},{1}");
    }
    public void OnClickGreen()
    {
        PlayerPrefs.SetString($"{part}Color{Index}", $"{0},{0},{1},{1}");
    }

}