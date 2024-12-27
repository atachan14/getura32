using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMoveEffectController : MonoBehaviour
{
    public float fadeDuration = 2f; // �t�F�[�h�A�E�g�ɂ����鎞��
    private Renderer objectRenderer;
    private Color initialColor;
    private Material objectMaterial;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectMaterial = objectRenderer.material; // �}�e���A���̃C���X�^���X���擾
            initialColor = objectMaterial.color; // ���̐F��ۑ�
            StartCoroutine(FadeOutAndDestroy());
            Debug.Log("ClickMoveEffectController:true");
        }
        else
        {
            Debug.LogWarning("Renderer��������܂���ł���");
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
            objectMaterial.color = newColor; // �X�V�͂����ōs��

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}