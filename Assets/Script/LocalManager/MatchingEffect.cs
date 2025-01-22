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
        RedPullZ(myTuraa);
        RedPullZ(target);

        redBoard.SetActive(true);
        myTuraa.GetComponent<OwnerPlayer>().RedStop();
    }

    public void OffRedEffect()
    {
        NotMatchingReturnZ(myTuraa);
        NotMatchingReturnZ(redTarget);
        redTarget = null;

        redBoard.SetActive(false);
        myTuraa.GetComponent<OwnerPlayer>().RedRelease();
    }

    void RedPullZ(GameObject turaa)
    {
        Vector3 oPos = turaa.transform.position;
        oPos.z = -9;
        turaa.transform.position = oPos;
        debugUI.AddDlList($"--RedPullZ--");
        debugUI.AddDlList($"myId:{NetworkManager.Singleton.LocalClientId} , turra.cId{turaa.GetComponent<NetworkObject>().OwnerClientId} , turra.transform.position:{turaa.transform.position}");
        Vector3 eyepos = turaa.transform.Find("Eye").transform.position;
        Vector3 legpos = turaa.transform.Find("Leg").transform.position;
        debugUI.AddDlList($"eyepos:{eyepos} , legpos{legpos} , redBoardpos:{redBoard.transform.position}");
    }

    void NotMatchingReturnZ(GameObject turaa)
    {
        Vector3 oPos = turaa.transform.position;
        oPos.z = 0;
        turaa.transform.position = oPos;
    }
}
