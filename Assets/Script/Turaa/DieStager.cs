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
        transform.localScale /= 2f;
        GetComponent<MatchingStatus>().IsAlive = false;
    }
}
