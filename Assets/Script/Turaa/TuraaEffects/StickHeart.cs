using UnityEngine;

public class StickHeart : MonoBehaviour
{
    float life = 3;
  
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up;
        life -= 1 * Time.deltaTime;
        if(life<0) Destroy(this);
    }
}
