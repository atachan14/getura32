using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMoveEffectController : MonoBehaviour
{
    public float fadeDuration = 2f; // フェードアウトにかける時間
    private Renderer objectRenderer;
    private Color initialColor;
    private Material objectMaterial;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectMaterial = objectRenderer.material; // マテリアルのインスタンスを取得
            initialColor = objectMaterial.color; // 元の色を保存
            StartCoroutine(FadeOutAndDestroy());
            Debug.Log("ClickMoveEffectController:true");
        }
        else
        {
            Debug.LogWarning("Rendererが見つかりませんでした");
            Destroy(gameObject);
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            Color newColor = initialColor;
            newColor.a = alpha;
            objectMaterial.color = newColor; // 更新はここで行う

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}