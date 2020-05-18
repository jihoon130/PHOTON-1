using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class BackMove : MonoBehaviourPunCallbacks
{
    private Move _Move;
    private PlayerAni _PlayerAni;

    public PhotonView PV;
    public Rigidbody rb;
    float b = 0f;
    bool d;
    // Start is called before the first frame update


    private void Start()
    {
        PV = GetComponentInParent<PhotonView>();
        _Move = GetComponentInParent<Move>();
        _PlayerAni = GetComponentInParent<PlayerAni>();
    }

    private void Update()
    {
        if (b >= 0.1f)
        {
            b -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Bullet"))
        //{
        //    string e = collision.gameObject.GetComponent<CastMove>().PV.ViewID.ToString();
        //    string r = gameObject.GetComponent<Move>().PV.ViewID.ToString();
        //
        //    if (e[0] == r[0])
        //        return;
        //
        //    Debug.Log("때린애 " + e);
        //    Debug.Log("맞은애 " + r);
        //
        //
        //    //PV.RPC("BackRPC", RpcTarget.All, collision.transform.position.x, collision.transform.position.y, collision.transform.position.z);
        //    //  collision.gameObject.GetComponent<CastMove>().PV.RPC("DestroyRPC", RpcTarget.All);
        //    //  ObjMoveback(collision);
        //}



        if (collision.gameObject.CompareTag("SpeedBullet"))
        {
            SpeedObjMoveback(collision);
        }
        if (collision.gameObject.CompareTag("SniperBullet"))
        {
            SpeedObjMoveback(collision, 10f);
        }

        if (b >= 0.1f && collision.gameObject.CompareTag("Wall"))
        {
            ObjMoveback2(collision);
        }

        if (collision.gameObject.CompareTag("Pok") && collision.gameObject.GetComponent<DestoryPok>().OK)
        {
            if (GetComponent<Move>().PV.IsMine)
                GetComponent<Move>().StopT += 5.0f;
            collision.gameObject.GetComponent<DestoryPok>().PV.RPC("DestroyRPC", RpcTarget.All);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            ObjMoveback4(other, 3f);
        }
    }
    private void ObjMoveback(Collision collision, float speed = 15.0f)
    {
        Vector3 pushdi = collision.transform.position - transform.position;
        pushdi = pushdi.normalized;
        rb.AddForce(pushdi * speed, ForceMode.Impulse);

        //rb.AddForce(collision.transform.forward * speed, ForceMode.Impulse);

    }
    private void SpeedObjMoveback(Collision collision, float speed = 5.0f)
    {
        _Move.SpeedSetting();
    }

    private void ObjMoveback2(Collision collision, float speed = 15.0f)
    {
        rb.AddForce(collision.transform.forward * speed, ForceMode.Impulse);
    }

    private void ObjMoveback3(Collision collision, float speed = 30.0f)
    {
        Vector3 pushdi = collision.transform.position - transform.position;
        pushdi = -pushdi.normalized;
        rb.AddForce(pushdi * speed, ForceMode.Impulse);
    }

    private void ObjMoveback4(Collider collision, float speed = 30.0f)
    {
        _PlayerAni._State = State.Dmg;
        Vector3 pushdi = collision.transform.position - transform.position;
        pushdi = -pushdi.normalized;
        rb.AddForce(pushdi * speed, ForceMode.Impulse);
    }

    public void Back1(float a, float b, float c)
    {
        PhotonNetwork.Instantiate("Hit", new Vector3(a, b, c), Quaternion.Euler(0, 0, 0));

        if (_Move.isPhoenix)
            return;

        _Move.PhoenixTimer();

        _PlayerAni._State = State.Dmg;
        rb.velocity = Vector3.zero;
        Vector3 pushdi = new Vector3(a, b, c) - transform.position;
        pushdi = -pushdi.normalized;
        pushdi.y = 0f;
        rb.AddForce(pushdi * 10.0f, ForceMode.Impulse);
    }

}