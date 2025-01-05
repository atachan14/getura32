using UnityEngine;

public class QolEffect : MonoBehaviour
{
    public GameObject ClickMoveEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickMove(Vector3 pos)
    {
        if (ClickMoveEffect == null)
        {
            Debug.LogError("ClickMoveEffectがnullです。削除された可能性があります。");
            return;
        }
      
        Instantiate(ClickMoveEffect, pos, Quaternion.identity,this.transform);
    }
}
