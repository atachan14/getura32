using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class MatchingEffect : MonoBehaviour
{
    public static MatchingEffect CI;

    [SerializeField] private GameObject pinkBoard;
    [SerializeField] private GameObject redBoard;
    [SerializeField] private GameObject stickEffectPrefab;

    List<GameObject> SOupList = new();
    Color pinkA = new (0.4f, 0, 0.2f, 0);
    //private GameObject redTarget;

    private GameObject myTuraa;

    private void Awake()
    {
        CI = this;
    }
    void Start()
    {
        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
    }
    public void RedEffect(bool b)
    {
        RedPinkSO();
        redBoard.SetActive(b);
    }

    public void OnRedEffect()
    {
        PullSO(myTuraa);
        PullSO(MatchingStatus.C.RedTuraa);

        redBoard.SetActive(true);
    }

    public void OffRedEffect()
    {
        ReturnSO(myTuraa);
        ReturnSO(MatchingStatus.C.RedTuraa);

        redBoard.SetActive(false);
    }

    public void PinkEffect()
    {
        RedPinkSO();
        List<(GameObject, int)> tupleList = MatchingStatus.C.PinkTupleList;
        pinkA.a = (tupleList.Count != 0) ? (tupleList.Count) * 2 / 100 : 0f;
        pinkBoard.GetComponent<SpriteRenderer>().color = pinkA;
    }

    //public void OnPinkEffect(List<GameObject> targetList)
    //{
    //    pinkTargetList = targetList;
    //    PullSO(myTuraa);
    //    foreach (GameObject target in pinkTargetList) PullSO(target);

    //    pinkBoard.SetActive(true);
    //    myTuraa.GetComponent<OwnerPlayer>().OnPinkSlow(targetList.Count);
    //}

    //public void OffPinkEffect()
    //{
    //    ReturnSO(myTuraa);
    //    foreach (GameObject target in pinkTargetList) ReturnSO(target);
    //    pinkTargetList.Clear();

    //    pinkBoard.SetActive(false);
    //    myTuraa.GetComponent<OwnerPlayer>().OffPinkSlow();
    //}

    void RedPinkSO()
    {
        foreach (GameObject t in SOupList) ReturnSO(t);
        SOupList.Clear();

        foreach ((GameObject p, int) tuple in MatchingStatus.C.PinkTupleList) SOupList.Add(tuple.p);
        SOupList.Add(MatchingStatus.C.RedTuraa);
        foreach (GameObject t in SOupList) PullSO(t);
    }

    void PullSO(GameObject turaa)
    {
        SpriteRenderer[] spriteRenderers = turaa.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.sortingOrder = 100;
        }
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
