using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorDisplay : MonoBehaviour
{
    private int selectTab;
    [SerializeField] TMP_InputField rField;
    [SerializeField] TMP_InputField bField;
    [SerializeField] TMP_InputField gField;
    [SerializeField] TMP_InputField nField;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSubmitColorR()
    {
        int colorR = int.Parse(rField.text);
        PlayerPrefs.SetInt($"Color,{selectTab},R", colorR);
        // changeColor atode kaku
    }

    public void OnSubmitColorB()
    {
        int colorB = int.Parse(bField.text);
        PlayerPrefs.SetInt($"Color,{selectTab},B", colorB);
        // changeColor atode kaku
    }
    public void OnSubmitColorG()
    {
        int colorG = int.Parse(gField.text);
        PlayerPrefs.SetInt($"Color,{selectTab},G", colorG);
        // changeColor atode kaku
    }
    public void OnSubmitColorN()
    {
        int colorN = int.Parse(nField.text);
        PlayerPrefs.SetInt($"Color,{selectTab},G", colorN);
    }
}