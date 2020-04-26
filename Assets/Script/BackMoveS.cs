using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class BackMoveS : MonoBehaviourPunCallbacks
{
    public Rigidbody rb;
    public PhotonView pv;
    // Start is called before the first frame update
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            pv.RPC("BackRPC", RpcTarget.AllBuffered, collision.transform.position.x, collision.transform.position.y, collision.transform.position.z);
        }

        if (collision.gameObject.CompareTag("SpeedBullet"))
        {
            DestroyBullet(collision);
        }
    }


    [PunRPC]
    void BackRPC(float a, float b, float c)
    {
        rb.velocity = Vector3.zero;
        Vector3 pushdi = new Vector3(a, b, c) - transform.position;
        pushdi = -pushdi.normalized;
        pushdi.y = 0f;
        rb.AddForce(pushdi * 3.0f, ForceMode.Impulse);
    }

    private void DestroyBullet(Collision collision)
    {
        collision.gameObject.GetComponent<CastMove>().PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
    }
}
