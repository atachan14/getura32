using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class InfoUI : MonoBehaviour
{
    private string targetName;
    [SerializeField] private TextMeshProUGUI nameTMP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTargetName(string value)
    {
        targetName = value;
        nameTMP.text = value;
    }
}
