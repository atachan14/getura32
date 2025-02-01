using UnityEngine;

using UnityEngine.EventSystems;
public class OpenCustamize : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckCollider();
        }
    }

    void CheckCollider()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
                if (hit.collider.CompareTag("Ball"))
                {
                    Debug.Log("ball");
                }
                else if (hit.collider.CompareTag("Eye"))
                {
                    Debug.Log("Eye");
                }
                else if (hit.collider.CompareTag("Leg"))
                {
                    Debug.Log("Leg");
                }           
        }
    }
}
