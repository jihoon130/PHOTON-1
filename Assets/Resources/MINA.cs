using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MINA : MonoBehaviour
{
  public  PhotonView pv;
    public bool OK=false;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }




    private void OnTriggerEnter(Collider other)
    {
     if(OK &&other.CompareTag("Player") && !other.GetComponent<Move>().PV.IsMine)
        {
            other.GetComponent<BackMove>().PV.RPC("ObjMoveback4RPC", RpcTarget.All, transform.position.x, transform.position.y, transform.position.z,2000f);
        }
    }

    public void OKTrue()
    {
        OK = true;
    }

    public void OKFalse()
    {
        OK = false;
    }
}
