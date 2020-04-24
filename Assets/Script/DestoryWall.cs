using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class DestoryWall : MonoBehaviourPunCallbacks
{
    public PhotonView Pv;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.tag);

        if(collision.collider.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Move>().isGround)
                collision.gameObject.GetComponent<Move>().isGround = false;
        }
        if (collision.collider.tag == "Bullet" || collision.collider.tag == "SpeedBullet")
        {
            collision.gameObject.GetComponent<CastMove>().PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);
}
