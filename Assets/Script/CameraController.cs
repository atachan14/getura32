using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float ScrollSize = 10;
    private bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveCamera(Vector3 direction)
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCameraSmoothly(direction));
        }
    }

    private IEnumerator MoveCameraSmoothly(Vector3 direction)
    {
        isMoving = true; // 移動中フラグをオン
        Vector3 startPosition = transform.position; // 開始位置
        Vector3 targetPosition = startPosition + direction * ScrollSize; // 目標位置
        float duration = 0.2f; // 移動にかける時間
        float elapsed = 0f;

        // 0.5秒かけて移動
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime; // 経過時間を更新
            float t = elapsed / duration; // 経過時間の割合
            transform.position = Vector3.Lerp(startPosition, targetPosition, t); // 線形補間で移動
            yield return null; // フレームごとに待機
        }

        transform.position = targetPosition; // 最終位置を補正
        isMoving = false; // 移動中フラグをオフ
    }
}