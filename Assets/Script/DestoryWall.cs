using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class DestoryWall : MonoBehaviourPunCallbacks
{
    public PhotonView Pv;


    private void Awake()
    {
        Pv = GetComponent<PhotonView>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Move>().isGround)
                collision.gameObject.GetComponent<Move>().isGround = false;
        }
        if (collision.collider.tag == "Bullet" || collision.collider.tag == "SpeedBullet")
        {
            collision.gameObject.GetComponent<CastMove>().PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            //Pv.RPC(HitEffectRPC(collision.transform.position);
            Pv.RPC("HitEffectRPC", RpcTarget.AllBuffered, collision.transform.position.x, 
                            collision.transform.position.y, collision.transform.position.z);
        }
    }


    [PunRPC]
    public void HitEffectRPC(float a, float b, float c)
    {
        PhotonNetwork.Instantiate("Hit", new Vector3(a, b, c), Quaternion.Euler(0, 0, 0));
    }

}
