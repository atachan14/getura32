using UnityEngine;

public class EffectController : MonoBehaviour
{
    public float minRotateSpeed = -100f; // 最小回転速度
    public float maxRotateSpeed = 100f;  // 最大回転速度
    private float rotateSpeed;

    public Vector2 scaleRangeX = new Vector2(0.8f, 1.2f); // Xの拡縮範囲
    public Vector2 scaleRangeY = new Vector2(0.8f, 1.2f); // Yの拡縮範囲
    private Vector3 startScale;
    private float scaleTimer;

    public float minAlpha = 0.2f; // 最小透明度
    public float maxAlpha = 1.0f; // 最大透明度
    private float alphaTimer;

    public Gradient colorGradient; // 色の変化パターン
    private float colorTimer;

    private SpriteRenderer spr;

    void Start()
    {
        rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);
        startScale = transform.localScale;
        scaleTimer = Random.Range(0f, 2f);
        alphaTimer = Random.Range(0f, 2f);
        colorTimer = Random.Range(0f, 2f);
        spr = GetComponent<SpriteRenderer>();

        // デフォルトのグラデーション（Inspectorで設定しない場合）
        if (colorGradient.colorKeys.Length == 0)
        {
            Debug.Log("PlzEffectChild noLength");
            colorGradient = new Gradient();
            colorGradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.red, 0f), new GradientColorKey(Color.blue, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
            );
        }
        else { Debug.Log("PlzEffectChild hasLength"); }
    }

    void Update()
    {
        // 🌀 回転
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

        // 📏 拡縮（X,Y個別）
        scaleTimer += Time.deltaTime;
        float scaleX = Mathf.Lerp(scaleRangeX.x, scaleRangeX.y, Mathf.PingPong(scaleTimer, 1));
        float scaleY = Mathf.Lerp(scaleRangeY.x, scaleRangeY.y, Mathf.PingPong(scaleTimer * 0.8f, 1));
        transform.localScale = new Vector3(startScale.x * scaleX, startScale.y * scaleY, 1);

        // 🌫️ 透明度変化
        alphaTimer += Time.deltaTime;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(alphaTimer, 1));

        // 🎨 色変化（グラデーションで徐々に変化）
        colorTimer += Time.deltaTime * 0.5f; // 速度調整
        Color newColor = colorGradient.Evaluate(colorTimer);
        newColor.a = alpha; // 透明度も適用
        Debug.Log($"{newColor}");
        spr.color = newColor;
    }
}
