using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GrenadeEffect : MonoBehaviourPunCallbacks
{
    public PhotonView PV;


    private CapsuleCollider CapColl;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        CapColl = GetComponent<CapsuleCollider>();
    }
    void Start()
    {
        StartCoroutine("Destroy");
    }


    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.5f);
        CapColl.enabled = false;
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }
}
