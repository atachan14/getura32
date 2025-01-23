using System.Drawing;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Inst;

    [SerializeField] private RoomSize roomSize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (IsServer && Inst == null) Inst = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
