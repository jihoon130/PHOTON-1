using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class DestoryEastEgg : MonoBehaviourPun
{
   public PhotonView pv;
    float OKTime = 0.0f;
    public bool OK = false; 
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    private void Update()
    {
        if(OKTime<=1.5f)
        {
            OKTime += Time.deltaTime;
        }
        else
        {
            OK = true;
        }
    }

    [PunRPC]
    void DestroyRPC() => Destroy(this.gameObject);
}
