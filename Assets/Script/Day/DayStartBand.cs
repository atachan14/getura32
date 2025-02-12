using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayStartBand : MonoBehaviour
{
    public static DayStartBand S;
    [SerializeField] GameObject band;
    [SerializeField] RectTransform gtlRect;
    [SerializeField] TextMeshProUGUI dayTMP;
    [SerializeField] RectTransform values;
    int day = 0;

    RectTransform bandRect;

    private void Awake()
    {
        S = this;
    }



    void SetupBand()
    {
        bandRect = band.GetComponent<RectTransform>();
        bandRect.localScale = new Vector3(bandRect.localScale.x, 0, bandRect.localScale.z);
        Debug.Log($"isBandRect:{bandRect != null}, localScale:{bandRect.localScale}");
    }

    public IEnumerator StartExe()
    {
        if (bandRect == null) SetupBand();

        band.SetActive(true);
        day++;


        values.anchoredPosition = new Vector2(0, values.anchoredPosition.y);
        yield return StartCoroutine(BandOpen());
        yield return StartCoroutine(GtlMoveIn());
        yield return StartCoroutine(GtlPop());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(DayVisible());
        StartCoroutine(ValuesMoveLeft());
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(BandClose());

    }

    IEnumerator BandOpen()
    {
        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newScaleY = Mathf.Lerp(0, 1, elapsedTime / duration);
            bandRect.localScale = new Vector3(bandRect.localScale.x, newScaleY, bandRect.localScale.z);
            yield return null;
        }
        bandRect.localScale = new Vector3(bandRect.localScale.x, 1, bandRect.localScale.z);
    }

    IEnumerator GtlMoveIn()
    {
        float elapsedTime = 0f;
        float duration = 1.5f;
        float startX = 800f;
        float endX = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newX = Mathf.Lerp(startX, endX, elapsedTime / duration);
            gtlRect.anchoredPosition = new Vector2(newX, gtlRect.anchoredPosition.y);
            yield return null;
        }
        gtlRect.anchoredPosition = new Vector2(endX, gtlRect.anchoredPosition.y);
    }

    IEnumerator GtlPop()
    {
        float elapsedTime = 0f;
        float duration = 0.05f;
        float startScale = 1;
        float maxScale = 1.1f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newScale = Mathf.Lerp(startScale, maxScale, elapsedTime / duration);
            gtlRect.localScale = new Vector3(newScale, newScale, gtlRect.localScale.z);
            yield return null;
        }
        gtlRect.localScale = new Vector3(maxScale, maxScale, gtlRect.localScale.z);

        yield return null;
        gtlRect.localScale = new Vector3(startScale, startScale, gtlRect.localScale.z);
    }

    IEnumerator DayVisible()
    {
        float elapsedTime = 0f;
        float duration = 2f;
        float startA = 0;
        float endA = 1f;
        Color c = dayTMP.color;
        dayTMP.text = $"Day {day}";

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newA = Mathf.Lerp(startA, endA, elapsedTime / duration);
            dayTMP.color = new Color(c.r, c.g, c.b, newA);
            yield return null;
        }
        dayTMP.color = new Color(c.r, c.g, c.b, endA);
    }

    IEnumerator ValuesMoveLeft()
    {
        float elapsedTime = 0f;
        float duration = 3f;
        float startX = 0;
        float endX = -1600;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newX = Mathf.Lerp(startX, endX, elapsedTime / duration);
            values.anchoredPosition = new Vector2(newX, values.anchoredPosition.y);
            yield return null;
        }
        values.anchoredPosition = new Vector2(endX, values.anchoredPosition.y);
    }

    IEnumerator BandClose()
    {
        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newScaleY = Mathf.Lerp(1, 0, elapsedTime / duration);
            bandRect.localScale = new Vector3(bandRect.localScale.x, newScaleY, bandRect.localScale.z);
            yield return null;
        }
        bandRect.localScale = new Vector3(bandRect.localScale.x, 0, bandRect.localScale.z);
    }
}
