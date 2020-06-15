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
    public AudioSource Audio;
    public AudioClip[] audioS;
    public Renderer rd;
    float b = 0f;
    bool d;
    // Start is called before the first frame update
    private void Awake()
    {
        Audio = GetComponentInChildren<AudioSource>();
    }

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


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            ObjMoveback2(other);
        }
        if (other.gameObject.CompareTag("SpeedBullet"))
        {
            ObjMoveback2(other, 2000f);
        }
    }

    private void ObjMoveback(Collider collision, float speed = 1500.0f)
    {
        Vector3 pushdi = collision.transform.position - transform.position;
        pushdi = pushdi.normalized;
        rb.AddForce(pushdi * speed, ForceMode.Impulse);

        //rb.AddForce(collision.transform.forward * speed, ForceMode.Impulse);

    }

    public void ObjMoveback2(Collider collision, float speed = 1000.0f)
    {
        PhotonNetwork.Instantiate("Hit", collision.transform.position, Quaternion.identity);
        if (_Move.isPhoenix || GetComponentInParent<Machinegun>().isMachineRay)
        {
            Destroy(collision.gameObject);
            return;
        }
        if (GetComponentInParent<Move>()._PlayerAni._State == State.Dash)
            return;


        int a;
        a = Random.Range(0, 2);
        Audio.clip = audioS[a];
        Audio.Play();

        GameObject[] pu = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < pu.Length; i++)
        {
            if (pu[i].GetComponentInParent<Move>().PV.ViewID.ToString().Substring(0, 1) == collision.gameObject.GetComponent<CastMove>().PV.ViewID.ToString().Substring(0,1))
            {
                GetComponent<Move>().Piguck = pu[i];
            }
        }

        rb.velocity = Vector3.zero;
        _PlayerAni._State = State.Dmg;

        rb.AddForce(collision.transform.forward * speed, ForceMode.Impulse);

        Destroy(collision.gameObject);
    }

    private void ObjMoveback3(Collision collision, float speed = 2000.0f)
    {
        if (GetComponentInParent<Machinegun>().isMachineRay)
            return;

        if (GetComponentInParent<Move>()._PlayerAni._State == State.Dash)
            return;

        GameObject[] pu = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < pu.Length; i++)
        {
            if (pu[i].GetComponentInParent<Move>().PV.ViewID.ToString().Substring(0, 1) == collision.gameObject.GetComponent<CastMove>().PV.ViewID.ToString().Substring(0, 1))
            {
                GetComponent<Move>().Piguck = pu[i];
            }
        }

        rb.velocity = Vector3.zero;
        _PlayerAni._State = State.Dmg;
        rb.AddForce(collision.transform.forward * speed, ForceMode.Impulse);
        Destroy(collision.gameObject);
    }

    private void ObjMoveback4(Collider collision, float speed = 3000.0f)
    {
        _PlayerAni._State = State.Dmg;
        Vector3 pushdi = collision.transform.position - transform.position;
        pushdi = -pushdi.normalized;
        rb.AddForce(pushdi * speed, ForceMode.Impulse);
    }

    //public void Back1(float a, float b, float c)
    //{
    //    PhotonNetwork.Instantiate("Hit", new Vector3(a, b, c), Quaternion.Euler(0, 0, 0));

    //    if (_Move.isPhoenix || GetComponentInParent<Machinegun>().isMachineRay)
    //        return;

    //    if (GetComponentInParent<Move>()._PlayerAni._State == State.Dash)
    //        return;

    //    _Move.PhoenixTimer();

    //    _PlayerAni._State = State.Dmg;
    //    rb.velocity = Vector3.zero;
    //    Vector3 pushdi = new Vector3(a, b, c) - transform.position;
    //    pushdi = -pushdi.normalized;
    //    pushdi.y = 0f;
    //    rb.AddForce(pushdi * 10.0f, ForceMode.Impulse);
    //}

    //[PunRPC]
    //public void BackRPC(float a, float b, float c)
    //{
    //    PhotonNetwork.Instantiate("Hit", new Vector3(a, b, c), Quaternion.Euler(0, 0, 0));

    //    if (GetComponentInParent<Machinegun>().isMachineRay)
    //        return;

    //    if (GetComponentInParent<Move>()._PlayerAni._State == State.Dash)
    //        return;

    //    _PlayerAni._State = State.Dmg;
    //    rb.velocity = Vector3.zero;
    //    Vector3 pushdi = new Vector3(a, b, c) - transform.position;
    //    pushdi = -pushdi.normalized;
    //    pushdi.y = 0f;
    //    rb.AddForce(pushdi * 30.0f, ForceMode.Impulse);
    //}

    public void hitOn()
    {
        rd.material.SetFloat("_Damaged", 1.0f);
    }
    public void hitOff()
    {
        rd.material.SetFloat("_Damaged", 0.0f);
    }
}