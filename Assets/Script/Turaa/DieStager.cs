using System.Collections;
using TMPro;
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

    public IEnumerator DieStaging()
    {
        Debug.Log("Start DieStaging");

        Vector3 startScale = transform.localScale;
        Vector3 targetScale = startScale * 0.5f;
        float duration = 1.0f;
        float elapsed = 0f;

        SpriteRenderer[] mySPRs = GetComponentsInChildren<SpriteRenderer>(true);
        Color startColor = mySPRs[0].color; // Å‰‚ÌF‚ğæ“¾
        Color targetColor = Color.blue; // Â‚É•Ï‰»

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            foreach (SpriteRenderer spr in mySPRs)
            {
                spr.color = Color.Lerp(startColor, targetColor, t);
            }

            yield return null;
        }

        transform.localScale = targetScale;
        foreach (SpriteRenderer spr in mySPRs)
        {
            spr.color = targetColor;
        }

        tag = "Untagged";
        Debug.Log("End DieStaging");
    }

}
