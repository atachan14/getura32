using UnityEngine;

public class DieStager : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DieStaging()
    {
        DebuLog.C.AddDlList("Start DieStaging");
        transform.localScale /= 2f;
        GetComponent<MatchingStatus>().IsAlive = false;
        DebuLog.C.AddDlList("End DieStaging");
    }
}
