using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class TentacleController : NetworkBehaviour
{
    public GameObject inactiveTentacle;
    public GameObject activeTentacle;
    private GameObject targetPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DeactivateTentacle();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            if (Input.GetMouseButtonDown(0)) ActivateTentacle();
            if (targetPlayer != null) KeepTentacle();
        }
    }

    public void KeepTentacle()
    {
        Vector3 targetPosition = targetPlayer.transform.position;
        targetPosition.z = activeTentacle.transform.position.z;

        Vector3 direction = targetPosition - activeTentacle.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        activeTentacle.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // �����𐳊m�Ɍv�Z���ĉ摜���������΂�
        float distance = direction.magnitude;
        float spriteHeight = activeTentacle.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        activeTentacle.transform.localScale = new Vector3(1, distance / spriteHeight, 1);
    }

    public void ActivateTentacle()
    {
        // ��A�N�e�B�u�G����B��
        inactiveTentacle.SetActive(false);

        // �A�N�e�B�u�G���\��
        activeTentacle.SetActive(true);

        // �}�E�X�ʒu�ł�Raycast�����s
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null&& hit.collider.CompareTag("CharacterClick"))
        {
            // �G��̃^�[�Q�b�g�ʒu���擾 (�q�b�g�n�_)
            Vector3 targetPosition = hit.point;
            targetPosition.z = activeTentacle.transform.position.z; // Z���W�𑵂���

            // �Ώۂ̕������v�Z
            Vector3 direction = targetPosition - activeTentacle.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            activeTentacle.transform.rotation = Quaternion.Euler(0, 0, angle); // �K�v�Ȃ�␳
         

            // �����𐳊m�Ɍv�Z���ĉ摜���������΂�
            float distance = direction.magnitude;
            float spriteHeight = activeTentacle.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
            activeTentacle.transform.localScale = new Vector3(1, distance / spriteHeight, 1);
            
            targetPlayer = hit.collider.gameObject;
        }
        else if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("click Info UI");
        }
        else
        {
            // �}�E�X���q�b�g���Ȃ���ΐG����\��
            DeactivateTentacle();
            Debug.Log("No hit detected. Tentacle deactivated.");
        }
    }






    public void DeactivateTentacle()
    {
        inactiveTentacle.SetActive(true);
        activeTentacle.SetActive(false);
        targetPlayer = null;
    }
}