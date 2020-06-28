using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GrenadeEffect : MonoBehaviourPunCallbacks
{
    public PhotonView PV;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        StartCoroutine("Destroy");
    }


    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}
