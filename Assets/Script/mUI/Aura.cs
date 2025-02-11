using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Aura : MonoBehaviour
{
    GameObject myTuraa;

    private void Start()
    {
        myTuraa = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
    }
    public void OnClickCantTalkAura()
    {
        myTuraa.GetComponent<MatchingStatus>().IsCant= !myTuraa.GetComponent<MatchingStatus>().IsCant;
    }
    public void OnClickPlzTalkAura()
    {
        myTuraa.GetComponent<MatchingStatus>().IsPlz = !myTuraa.GetComponent<MatchingStatus>().IsPlz;
    }


}
