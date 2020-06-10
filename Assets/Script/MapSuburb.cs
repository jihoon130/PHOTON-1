using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MapSuburb : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        //collision.gameObject.GetComponent<Move>().PV.RPC("ResetPosRPC", RpcTarget.All);
        if(collision.collider.CompareTag("Attack1"))
        {
            collision.gameObject.GetComponentInParent<Move>().DieTrue();
        }
    }
}
