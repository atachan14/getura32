using Unity.Netcode;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MatchingEffect : MonoBehaviour
{
    [SerializeField] GameObject redBoard;
    private GameObject target;
    private GameObject myTuraa;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupBoard(redBoard);
        
    }

    void SetupBoard(GameObject board)
    {
        board.SetActive(false);
        SpriteRenderer redSpriteRenderer = board.GetComponent<SpriteRenderer>();
        Color boardColor = redSpriteRenderer.color;
        boardColor.a = 0.6f; // AlphaílÇê›íËÅi0.0 - 1.0Åj
        redSpriteRenderer.color = boardColor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnRedEffect(GameObject target)
    {

        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        this.target=target;
        RedPullZ(myTuraa);
        RedPullZ(target);

        redBoard.SetActive(true);
        myTuraa.GetComponent<OwnerPlayer>().RedStop();
    }

    public void OffRedEffect()
    {
        NotMatchingReturnZ(myTuraa);
        NotMatchingReturnZ(target);
    }

    void RedPullZ(GameObject turaa)
    {
        Vector3 oPos = turaa.transform.position;
        oPos.z = -9;
        turaa.transform.position = oPos;
    }

    void NotMatchingReturnZ(GameObject turaa)
    {
        Vector3 oPos = turaa.transform.position;
        oPos.z = 0;
        turaa.transform.position = oPos;
    }
}
