using UnityEngine;

public class StickHeart : MonoBehaviour
{
    float life = 3;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime *0.7f;
        life -= Time.deltaTime;
        if (life < 0) Destroy(gameObject);
    }
}