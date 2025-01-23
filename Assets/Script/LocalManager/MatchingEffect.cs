using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class MatchingEffect : MonoBehaviour
{
    [SerializeField] private TargetInfoManager targetInfo;
    [SerializeField] private InputManager inputManager;

    [SerializeField] private GameObject redBoard;
    private GameObject redTarget;

    [SerializeField] private GameObject pinkBoard;
    private List<GameObject> pinkTargetList;

    private GameObject myTuraa;

    void Start()
    {
        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        SetupBoard(redBoard);
        SetupBoard(pinkBoard);
    }

    void SetupBoard(GameObject board)
    {
        board.SetActive(false);
        SpriteRenderer redSpriteRenderer = board.GetComponent<SpriteRenderer>();
        Color boardColor = redSpriteRenderer.color;
        boardColor.a = 0.5f; // AlphaílÇê›íËÅi0.0 - 1.0Åj
        redSpriteRenderer.color = boardColor;
    }

    void Update()
    {

    }
    public void OnRedEffect(GameObject target)
    {
        DebugWndow.CI.AddDlList("OnRedEffect");
        redTarget = target;
        PullSO(myTuraa);
        PullSO(redTarget);

        redBoard.SetActive(true);
        myTuraa.GetComponent<OwnerPlayer>().IsRedStop=true;
        myTuraa.GetComponent<TentacleController>().IsRedStop=true;
        inputManager.IsRedStop=true;
    }

    public void OffRedEffect()
    {
        ReturnSO(myTuraa);
        ReturnSO(redTarget);
        redTarget = null;

        redBoard.SetActive(false);
        myTuraa.GetComponent<OwnerPlayer>().IsRedStop = false;
        myTuraa.GetComponent<TentacleController>().IsRedStop = false;
        inputManager.IsRedStop = false;
    }

    public void OnPinkEffect(List<GameObject> targetList)
    {
        DebugWndow.CI.AddDlList("---------OnPinkEffect");

        pinkTargetList = targetList;
        PullSO(myTuraa);
        foreach (GameObject target in pinkTargetList) PullSO(target);

        pinkBoard.SetActive(true);
        myTuraa.GetComponent<OwnerPlayer>().OnPinkSlow();

        DebugWndow.CI.AddDlList("---------OnPinkEffect end");
    }

    public void OffPinkEffect()
    {
        ReturnSO(myTuraa);
        foreach (GameObject target in pinkTargetList) ReturnSO(target);
        pinkTargetList.Clear();

        pinkBoard.SetActive(false);
        myTuraa.GetComponent<OwnerPlayer>().OffPinkSlow();
    }

    void PullSO(GameObject turaa)
    {
        SpriteRenderer[] spriteRenderers = turaa.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.sortingOrder += 10; 
        }
        DebugWndow.CI.AddDlList($"PullSO myId:{NetworkManager.Singleton.LocalClientId} , turra.cId{turaa.GetComponent<NetworkObject>().OwnerClientId} ,  spriteRenderers[0].sortingOrder:{spriteRenderers[0].sortingOrder}");
    }

    void ReturnSO(GameObject turaa)
    {
        SpriteRenderer[] spriteRenderers = turaa.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.sortingOrder -= 10;
        }
    }
}
