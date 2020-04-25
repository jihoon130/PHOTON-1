﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BackMove : MonoBehaviourPunCallbacks
{
    private Move _Move;

    public PhotonView PV;
    public Rigidbody rb;
    float b = 0f;

    // Start is called before the first frame update


    private void Start()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        _Move = GetComponent<Move>();
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            ObjMoveback(collision);
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
            ObjMoveback4(other);
        }
    }
    private void ObjMoveback(Collision collision, float speed = 15.0f)
    {
        b = 2.0f;
        rb.AddForce(collision.transform.forward * speed, ForceMode.Impulse);
        collision.gameObject.GetComponent<CastMove>().PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
    }
    private void SpeedObjMoveback(Collision collision, float speed = 5.0f)
    {
        b = 2.0f;
        rb.AddForce(collision.transform.forward * speed, ForceMode.Impulse);
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
}
