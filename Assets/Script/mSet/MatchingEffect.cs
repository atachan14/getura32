using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class MatchingEffect : MonoBehaviour
{
    public static MatchingEffect CI;
    [SerializeField] private TargetInfoManager targetInfo;
    [SerializeField] private InputManager inputManager;

    [SerializeField] private GameObject purpleBoard;
    [SerializeField] private GameObject redBoard;
    [SerializeField] private GameObject stickEffectPrefab;

    private List<GameObject> purpleTargetList = new();
    private GameObject redTarget;

    private GameObject myTuraa;
    private MatchingStatus mStatus;

    private void Awake()
    {
        CI = this;
    }
    void Start()
    {
        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        mStatus = myTuraa.GetComponent<MatchingStatus>();
    }

    void SetupBoard(GameObject board)
    {
        board.SetActive(false);
        SpriteRenderer redSpriteRenderer = board.GetComponent<SpriteRenderer>();
        Color boardColor = redSpriteRenderer.color;
        boardColor.a = 0.5f; // AlphaílÇê›íËÅi0.0 - 1.0Åj
        redSpriteRenderer.color = boardColor;
    }

   
    public void RedEffectSelecter(GameObject redTarget)
    {
        this.redTarget = redTarget;
        if (redTarget) OnRedEffect();
        else OffRedEffect();
    }

    public void OnRedEffect()
    {
        PullSO(myTuraa);
        PullSO(mStatus.RedTarget);

        redBoard.SetActive(true);
        inputManager.IsRedStop = true;
    }

    public void OffRedEffect()
    {
        ReturnSO(myTuraa);
        ReturnSO(mStatus.RedTarget);

        redBoard.SetActive(false);
        inputManager.IsRedStop = false;
    }

    public void OnPinkEffect(List<GameObject> targetList)
    {
        PullSO(myTuraa);
        foreach (GameObject target in targetList) PullSO(target);

        purpleBoard.SetActive(true);
        myTuraa.GetComponent<TuraaWalker>().OnPinkSlow(targetList.Count);
    }

    public void OffPinkEffect()
    {
        ReturnSO(myTuraa);
        foreach (GameObject target in targetList) ReturnSO(target);
        targetList.Clear();

        purpleBoard.SetActive(false);
        myTuraa.GetComponent<TuraaWalker>().OffPinkSlow();
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
