using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nextTMP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

   

    public void NextExe()
    {
        SLD.SingleLoad(SNM.Day);
    }
}
