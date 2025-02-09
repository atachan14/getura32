using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StickEffect : NetworkBehaviour
{
    public static StickEffect C;
    [SerializeField] GameObject StickingHeartPrefab;

    Vector3 firstoffset = new(0.7f, -1.5f, 0);
    Vector3 secondoffset = new(-0.4f, -1f, 0);

    public bool Sticking { get; set; }

    float firstCount = 4;
    float secondCount = 2;

    private void Awake()
    {
        if (IsOwner) C = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Sticking) Exe();
    }
    [ServerRpc]
    public void StickingServerRpc(bool b)
    {
        StickingClientRpc(b);
    }
    [ClientRpc]
    void StickingClientRpc(bool b)
    {
        Sticking = b;
    }

    void Exe()
    {
        if (firstCount > 4)
        {
            firstCount = 0;
            GameObject sh = Instantiate(StickingHeartPrefab, transform);
            sh.transform.position += firstoffset;
        }
        if (secondCount > 4)
        {
            secondCount = 0;
            GameObject sh2 = Instantiate(StickingHeartPrefab, transform);
            sh2.transform.position += secondoffset;
        }

        firstCount += 1 * Time.deltaTime;
        secondCount += 1 * Time.deltaTime;
    }

}