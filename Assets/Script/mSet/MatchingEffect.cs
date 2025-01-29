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

    [SerializeField] private GameObject redBoard;
    private GameObject redTarget;

    [SerializeField] private GameObject purpleBoard;
    private List<GameObject> purpleTargetList;

    [SerializeField] private GameObject stickEffectPrefab;
    public GameObject Partner { get; set; }

    private GameObject myTuraa;

    private void Awake()
    {
        CI= this;
    }

    void Start()
    {
        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        //SetupBoard(redBoard);
        //SetupBoard(pinkBoard);
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
        if (Partner != null) myTuraa.GetComponent<OwnerPlayer>().StickMove(Partner);
    }

    public void ChangePartner(GameObject newPartner)
    {
        if (Partner != null)
        {
            ulong oldPartnerId = Partner.GetComponent<NetworkObject>().OwnerClientId;
            SplitServerRpc(oldPartnerId);
        }
        Partner = newPartner;
    }

    public void Split()
    {
        ulong oldPartnerId = Partner.GetComponent<NetworkObject>().OwnerClientId;
        SplitServerRpc(oldPartnerId);
        Partner = null;
    }

    [ServerRpc]
    public void SplitServerRpc(ulong NTRId)
    {
        SplitClientRpc(NTRId);
    }

    [ClientRpc]
    public void SplitClientRpc(ulong NTRId)
    {
        if (NetworkManager.Singleton.LocalClientId == NTRId)
        {
            Partner = null;
        }
    }


    public void OnRedEffect(GameObject target)
    {
        redTarget = target;
        PullSO(myTuraa);
        PullSO(redTarget);

        redBoard.SetActive(true);
        inputManager.IsRedStop=true;
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

        purpleBoard.SetActive(true);
        myTuraa.GetComponent<OwnerPlayer>().OnPinkSlow(targetList.Count);
    }

    public void OffPinkEffect()
    {
        ReturnSO(myTuraa);
        foreach (GameObject target in purpleTargetList) ReturnSO(target);
        purpleTargetList.Clear();

        purpleBoard.SetActive(false);
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
