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

    [SerializeField] private GameObject HeartPrefab;
    private GameObject matchingTarget;
    private int which;

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
        boardColor.a = 0.5f; // Alpha値を設定（0.0 - 1.0）
        redSpriteRenderer.color = boardColor;
    }

    void Update()
    {
        if (matchingTarget != null) MatchingDance();
    }

    public void SetMatchingTarget(GameObject target,int which)
    {
        matchingTarget = target;
        this.which = which;
    }

    void MatchingDance()
    {


    }


    public void OnRedEffect(GameObject target)
    {
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
        pinkTargetList = targetList;
        PullSO(myTuraa);
        foreach (GameObject target in pinkTargetList) PullSO(target);

        pinkBoard.SetActive(true);
        myTuraa.GetComponent<OwnerPlayer>().OnPinkSlow();
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
