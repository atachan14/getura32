using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StickEffect : NetworkBehaviour
{
    public static StickEffect C;
    [SerializeField] GameObject StickingHeartPrefab;

    Vector3 offset = new (1f, -1f, 0);
    bool sticking;

    int firstCount = 4;
    int secondCount = 2;

    private void Awake()
    {
        if (IsOwner) C = this;
    }
  
    // Update is called once per frame
    void Update()
    {
        if (sticking) Exe();
    }
    [ServerRpc]
    public void StickingServerRpc(bool b)
    {
        StickingClientRpc(b);
    }
    [ClientRpc]
    void StickingClientRpc(bool b)
    {
        sticking = b;
    }

    void Exe()
    {
        if (firstCount > 4)
        {
            GameObject sh = Instantiate(StickingHeartPrefab);
            sh.transform.position += offset;
        }
        if (secondCount > 4)
        {
            GameObject sh2 = Instantiate(StickingHeartPrefab);
            sh2.transform.position -= offset;
        }

        firstCount++;
        secondCount++;

    }

}
