using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class MatchingEffect : MonoBehaviour
{
    [SerializeField] DebugWndow debugUI;

    [SerializeField] GameObject redBoard;
    private GameObject redTarget;
    private GameObject myTuraa;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        SetupBoard(redBoard);
    }

    void SetupBoard(GameObject board)
    {
        board.SetActive(false);
        SpriteRenderer redSpriteRenderer = board.GetComponent<SpriteRenderer>();
        Color boardColor = redSpriteRenderer.color;
        boardColor.a = 0.7f; // AlphaílÇê›íËÅi0.0 - 1.0Åj
        redSpriteRenderer.color = boardColor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnRedEffect(GameObject target)
    {
        this.redTarget = target;
        RedPullSO(myTuraa);
        RedPullSO(target);

        redBoard.SetActive(true);
        myTuraa.GetComponent<OwnerPlayer>().RedStop();
    }

    public void OffRedEffect()
    {
        NotMatchingReturnSO(myTuraa);
        NotMatchingReturnSO(redTarget);
        redTarget = null;

        redBoard.SetActive(false);
        myTuraa.GetComponent<OwnerPlayer>().RedRelease();
    }

    void RedPullSO(GameObject turaa)
    {
        debugUI.AddDlList($"--RedPullZ--");
        
        SpriteRenderer[] spriteRenderers = turaa.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.sortingOrder = 10; 
        }
        debugUI.AddDlList($"myId:{NetworkManager.Singleton.LocalClientId} , turra.cId{turaa.GetComponent<NetworkObject>().OwnerClientId} ,  spriteRenderers[0].sortingOrder:{spriteRenderers[0].sortingOrder}");

    }

    void NotMatchingReturnSO(GameObject turaa)
    {
        SpriteRenderer[] spriteRenderers = turaa.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.sortingOrder = 0;
        }
    }
}
