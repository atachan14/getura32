using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorDisplay : MonoBehaviour
{
    public int Index { get; set; }
    [SerializeField] TMP_InputField rField;
    [SerializeField] TMP_InputField bField;
    [SerializeField] TMP_InputField gField;
    [SerializeField] TMP_InputField aField;
  
    public void SetupIndex( int i)
    {
        Index = i;
        MyColorToSetField();
    }

    void MyColorToSetField()
    {
        string savedColor = PlayerPrefs.GetString($"{OpenShapeDisplay.L.Part}Color{Index}", "1,1,1,1");
        savedColor = float.TryParse(savedColor ,out float result) ? savedColor : "0,0,0,0";
        Debug.Log($"savedColor:{savedColor}");
        string[] rgba = savedColor.Split(',');
        rField.text = rgba[0];
        gField.text = rgba[1];
        bField.text = rgba[2];
        aField.text = rgba[3];
        TabManager.L.colorTabs[Index].GetComponent<Image>().color
            = new Color(float.Parse(rgba[0]), float.Parse(rgba[1]), float.Parse(rgba[2]), float.Parse(rgba[3]));
    }


    public void OnSubmitColor()
    {
        Color myColor = new 
        (
            float.TryParse(rField.text, out float rresult) ? rresult : 0,
            float.TryParse(gField.text, out float gresult) ? gresult : 0,
            float.TryParse(bField.text, out float bresult) ? bresult : 0,
            float.TryParse(aField.text, out float aresult) ? aresult : 1
        );

        PlayerPrefs.SetString($"{OpenShapeDisplay.L.Part}Color{Index}", $"{myColor.r},{myColor.g},{myColor.b},{myColor.a}");
        Debug.Log($"OnSubmitColor:{myColor.r},{myColor.g},{myColor.b},{myColor.a}");
        MenuDammySpriter.L.ChangeColor(OpenShapeDisplay.L.Part, Index);
    }


    public void OnClickRed()
    {
        PlayerPrefs.SetString($"{OpenShapeDisplay.L.Part}Color{Index}", $"25,0,0,25");
        MyColorToSetField();
    }
    public void OnClickBlue()
    {
        PlayerPrefs.SetString($"{OpenShapeDisplay.L.Part}Color{Index}", $"{0},{0},{25},{25}");
        MyColorToSetField();
    }
    public void OnClickGreen()
    {
        PlayerPrefs.SetString($"{OpenShapeDisplay.L.Part}Color{Index}", $"{0},{10},{0},{25}");
        MyColorToSetField();
    }
    public void OnClickYellow()
    {
        PlayerPrefs.SetString($"{OpenShapeDisplay.L.Part}Color{Index}", $"{25},{25},{0},{25}");
        MyColorToSetField();
    }
}