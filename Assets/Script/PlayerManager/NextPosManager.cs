using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPosManager : MonoBehaviour
{
    public OwnerPlayer pController;
    public GameObject pClickMoveEffect;
    public GameObject pClickMoveMark;

    private GameObject pCMM;
    private float NextPos;

    private Vector3 pNextPos;
    // Start is called before the first frame update
    void Start()
    {
        pCMM = Instantiate(pClickMoveMark, transform.position, Quaternion.identity,transform.parent);
        pCMM.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClickMoveQol(Vector3 nextPos)
    {
        transform.position = nextPos;

        
        pCMM.transform.position = nextPos;
        pCMM.SetActive(true);
    }

    public void ClickMoveEnd()
    {
        pCMM.SetActive(false);
    }
}
