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
        if (collision.collider.tag == "Bullet" || collision.collider.tag == "ReflectBullet" || collision.collider.tag == "SpeedBullet")
        {
            collision.gameObject.GetComponent<CastMove>().PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
        else if (collision.collider.tag == "Player")
        {
            collision.gameObject.GetComponent<Move>().PV.RPC("ResetPos", RpcTarget.AllBuffered);
        }
    }
}
