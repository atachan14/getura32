using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class MatchingEffect : MonoBehaviour
{
    [SerializeField] private DebugWndow debugUI;

    [SerializeField] private GameObject redBoard;
    private GameObject redTarget;
    [SerializeField] private GameObject pinkBoard;
    private GameObject pinkTarget;
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
        redTarget = target;
        PullSO(myTuraa);
        PullSO(redTarget);

        redBoard.SetActive(true);
        myTuraa.GetComponent<OwnerPlayer>().RedStop();
    }

    public void OffRedEffect()
    {
        ReturnSO(myTuraa);
        ReturnSO(redTarget);
        redTarget = null;

        redBoard.SetActive(false);
        myTuraa.GetComponent<OwnerPlayer>().RedRelease();
    }

    public void OnPinkEffect()
    {
        PullSO(myTuraa);
        PullSO(redTarget);

        pinkBoard.SetActive(true);
        myTuraa.GetComponent<OwnerPlayer>().OnPinkSlow();
    }

    public void OffPinkEffect()
    {
        ReturnSO(myTuraa);
        ReturnSO(redTarget);
        pinkTarget = null;

        pinkBoard.SetActive(false);
        myTuraa.GetComponent<OwnerPlayer>().OffPinkSlow();
    }

    void PullSO(GameObject turaa)
    {
        SpriteRenderer[] spriteRenderers = turaa.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.sortingOrder = 10; 
        }
        debugUI.AddDlList($"myId:{NetworkManager.Singleton.LocalClientId} , turra.cId{turaa.GetComponent<NetworkObject>().OwnerClientId} ,  spriteRenderers[0].sortingOrder:{spriteRenderers[0].sortingOrder}");

    }

    void ReturnSO(GameObject turaa)
    {
        SpriteRenderer[] spriteRenderers = turaa.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.sortingOrder = 0;
        }
    }
}
