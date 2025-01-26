using TMPro;
using UnityEngine;

public class MenuNamePlate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTMP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTMP(string newName)
    {
        nameTMP.text = newName;
    }
}
