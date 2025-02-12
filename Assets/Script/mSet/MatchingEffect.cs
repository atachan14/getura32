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

    //void SetupBoard(GameObject board)
    //{
    //    board.SetActive(false);
    //    SpriteRenderer redSpriteRenderer = board.GetComponent<SpriteRenderer>();
    //    Color boardColor = redSpriteRenderer.color;
    //    boardColor.a = 0.5f; // AlphaílÇê›íËÅi0.0 - 1.0Åj
    //    redSpriteRenderer.color = boardColor;
    //}

   
    public void RedEffect(bool b)
    {
        AllTuraaSoSelect();
        redBoard.SetActive(b);
    }

    //public void OnRedEffect(GameObject target)
    //{
    //    //PullSO(myTuraa);
    //    //PullSO(target);

    //    redBoard.SetActive(true);
    //}

    //public void OffRedEffect(GameObject target)
    //{
    //    //ReturnSO(myTuraa);
    //    //ReturnSO(target);

    //    redBoard.SetActive(false);
    //}

    public void OnPinkEffect(List<GameObject> targetList)
    {
        purpleTargetList = targetList;
        //PullSO(myTuraa);
        //foreach (GameObject target in purpleTargetList) PullSO(target);
        AllTuraaSoSelect();

        purpleBoard.SetActive(true);
        myTuraa.GetComponent<TuraaWalker>().OnPinkSlow(targetList.Count);
    }

    public void OffPinkEffect()
    {
        //ReturnSO(myTuraa);
        //foreach (GameObject target in purpleTargetList) ReturnSO(target);
        purpleTargetList.Clear();
        AllTuraaSoSelect();

        purpleBoard.SetActive(false);
        myTuraa.GetComponent<TuraaWalker>().OffPinkSlow();
    }

    void PullSO(GameObject turaa)
    {
        SpriteRenderer[] spriteRenderers = turaa.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.sortingOrder += 100;
        }
    }

    void ReturnSO(GameObject turaa)
    {
        SpriteRenderer[] spriteRenderers = turaa.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.sortingOrder -= 100;
        }
    }

    void AllTuraaSoSelect()
    {
        foreach (var Client in NetworkManager.Singleton.ConnectedClients)
        {
            Client.Value.PlayerObject.GetComponent<SoManager>().SoSelect();
        }
    }
}
