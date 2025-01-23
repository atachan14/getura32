using TMPro;
using UnityEngine;

public class RoomSize : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI robbySizeTMP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reflection(int size)
    {
        robbySizeTMP.text = size.ToString();
    }

}
