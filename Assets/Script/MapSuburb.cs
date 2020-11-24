using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MapSuburb : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Attack1"))
        {
            collision.gameObject.GetComponentInParent<Move>().DieTrue();
            //Create_WaterEffect(collision.gameObject.transform);
        }
    }

    private void Create_WaterEffect(Transform PointTarget)
    {
        PhotonNetwork.Instantiate("FX_RingOut", PointTarget.position, Quaternion.identity);
    }
}
