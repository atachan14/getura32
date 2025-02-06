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

    [SerializeField] private GameObject pinkBoard;
    [SerializeField] private GameObject redBoard;
    [SerializeField] private GameObject stickEffectPrefab;

    private List<GameObject> purpleTargetList;
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
    }

  

    void Update()
    {

    }

    public void OnRedEffect(GameObject target)
    {
        redTarget = target;
        PullSO(myTuraa);
        PullSO(redTarget);

        redBoard.SetActive(true);
        inputManager.IsRedStop = true;
    }

    public void OffRedEffect()
    {
        ReturnSO(myTuraa);
        ReturnSO(redTarget);
        redTarget = null;

        redBoard.SetActive(false);
        inputManager.IsRedStop = false;
    }

    public void OnPinkEffect(List<GameObject> targetList)
    {
        purpleTargetList = targetList;
        PullSO(myTuraa);
        foreach (GameObject target in purpleTargetList) PullSO(target);

        pinkBoard.SetActive(true);
        myTuraa.GetComponent<OwnerPlayer>().OnPinkSlow(targetList.Count);
    }

    public void OffPinkEffect()
    {
        ReturnSO(myTuraa);
        foreach (GameObject target in purpleTargetList) ReturnSO(target);
        purpleTargetList.Clear();

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
