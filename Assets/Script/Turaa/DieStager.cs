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

        Vector3 start = transform.localScale;
        Vector3 target = start * 0.5f; // 目標のサイズ
        float duration = 1.0f; // 1秒かけて縮む
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, target, elapsed / duration);
            yield return null;
        }

        transform.localScale = target; // 最後にピタッと合わせる

        DieColor();
        GetComponent<MatchingStatus>().IsAlive = false;

       Debug.Log("End DieStaging");
    }

    void DieColor()
    {
        SpriteRenderer[] mySPRs = GetComponentsInChildren<SpriteRenderer>(true);
        foreach (SpriteRenderer spr in mySPRs)
        {
            spr.color = Color.blue;
        }
    }
}
