using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BackMove : MonoBehaviourPunCallbacks
{
    public GameObject Not;
    public PhotonView PV;
    public Rigidbody rb;
    float b = 0f;
    // Start is called before the first frame update

    private void Update()
    {
        if(b>=0.1f)
        {
            b -= Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            rb.AddForce(new Vector3(100.0f,100.0f,100.0f));
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            ObjMoveback(other);
        }

        if (b >= 0.1f && other.gameObject.CompareTag("Wall"))
        {
            ObjMoveback2(other);
            other.gameObject.GetComponent<DestoryWall>().Pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }

        if (other.gameObject.CompareTag("Wall2"))
        {
            ObjMoveback3(other);
        }

        if (other.gameObject != Not &&  other.gameObject.CompareTag("Head"))
        {
            ObjMoveback4(other);
        }
    }
    private void ObjMoveback(Collider collision, float speed = 15.0f)
    {
        b = 2.0f;
        rb.AddForce(collision.transform.forward * speed, ForceMode.Impulse);
        collision.gameObject.GetComponent<CastMove>().PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
    }

    private void ObjMoveback2(Collider collision, float speed = 15.0f)
    {
        rb.AddForce(collision.transform.forward * speed, ForceMode.Impulse);
    }

    private void ObjMoveback3(Collider collision, float speed = 100.0f)
    {
        rb.AddForce(collision.transform.forward * speed, ForceMode.Impulse);
    }

    private void ObjMoveback4(Collider collision, float speed = 20.0f)
    {
        b = 2.0f;
        Vector3 pushdi = collision.transform.position - transform.position;
        pushdi =- pushdi.normalized;
        rb.AddForce(pushdi * speed, ForceMode.Impulse);
    }
}
