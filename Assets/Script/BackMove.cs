using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class BackMove : MonoBehaviourPunCallbacks
{
    private Move _Move;
    public CanvasGroup cg;
    public PhotonView PV;
    public Rigidbody rb;
    float b = 0f;
    bool d;
    // Start is called before the first frame update


    private void Start()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        _Move = GetComponent<Move>();
        cg = GameObject.Find("Sorry").GetComponent<CanvasGroup>();
    }

    private void Update()
    {
       

        if(b>=0.1f)
        {
            b -= Time.deltaTime;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("ReflectBullet"))
        {
            string e = collision.gameObject.GetComponent<CastMove>().PV.ViewID.ToString();
            string r = gameObject.GetComponent<Move>().PV.ViewID.ToString();

            if (e[0] == r[0])
                return;

            PV.RPC("BackRPC", RpcTarget.AllBuffered, collision.transform.position.x, collision.transform.position.y, collision.transform.position.z);
            collision.gameObject.GetComponent<CastMove>().PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            //  ObjMoveback(collision);
        }



        if (collision.gameObject.CompareTag("SpeedBullet"))
        {
            SpeedObjMoveback(collision);
        }

        if (b >= 0.1f && collision.gameObject.CompareTag("Wall"))
        {
            ObjMoveback2(collision);
        }

        if(collision.gameObject.CompareTag("Pok") && collision.gameObject.GetComponent<DestoryPok>().OK)
        {
            if (GetComponent<Move>().PV.IsMine)
                GetComponent<Move>().StopT += 5.0f; 
            collision.gameObject.GetComponent<DestoryPok>().PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            ObjMoveback4(other, 3f);
        }

        if (other.gameObject.CompareTag("Sorry") && other.gameObject.GetComponent<DestoryEastEgg>().OK)
        {
            other.gameObject.GetComponent<DestoryEastEgg>().pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
            cg.alpha = 1;
        }
    }
    private void ObjMoveback(Collision collision, float speed = 15.0f)
    {
        

        Vector3 pushdi = collision.transform.position - transform.position;
        pushdi = pushdi.normalized;
        rb.AddForce(pushdi * speed, ForceMode.Impulse);

        //rb.AddForce(collision.transform.forward * speed, ForceMode.Impulse);
        collision.gameObject.GetComponent<CastMove>().PV.RPC("DestroyRPC", RpcTarget.AllBuffered);

    }
    private void SpeedObjMoveback(Collision collision, float speed = 5.0f)
    {
        collision.gameObject.GetComponent<CastMove>().PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
        _Move.PV.RPC("SpeedSetting", RpcTarget.AllBuffered);
    }

    private void ObjMoveback2(Collision collision, float speed = 15.0f)
    {
        rb.AddForce(collision.transform.forward * speed, ForceMode.Impulse);
    }

    private void ObjMoveback3(Collision collision, float speed = 30.0f)
    {
      
        Vector3 pushdi = collision.transform.position - transform.position;
        pushdi =- pushdi.normalized;
        rb.AddForce(pushdi * speed, ForceMode.Impulse);
    }

    private void ObjMoveback4(Collider collision, float speed = 30.0f)
    {

        Vector3 pushdi = collision.transform.position - transform.position;
        pushdi = -pushdi.normalized;
        rb.AddForce(pushdi * speed, ForceMode.Impulse);
    }

    [PunRPC]
    void BackRPC(float a,float b,float c)
    {
        rb.velocity = Vector3.zero;
        Vector3 pushdi = new Vector3(a,b,c) - transform.position;
        pushdi =- pushdi.normalized;
        pushdi.y = 0f;
        rb.AddForce(pushdi * 15.0f, ForceMode.Impulse);
    }
}
