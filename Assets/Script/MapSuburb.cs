using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MapSuburb : MonoBehaviourPunCallbacks
{


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Attack1"))
        {
            collision.gameObject.GetComponentInParent<Move>().DieTrue();
        }
    }
}
