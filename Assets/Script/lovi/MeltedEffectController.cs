using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltedEffectController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public PlayerStatus pSta;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTransparency(float alpha)
    {
        // alpha値を0～1に制限
        alpha = Mathf.Clamp01(alpha);

        // 色のアルファ値を変更
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
