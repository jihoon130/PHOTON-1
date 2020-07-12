using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CastMove : MonoBehaviourPunCallbacks
{
    public enum BulletMode { Attack, Machinegun, Grenade}
    public BulletMode _BulletMode = BulletMode.Attack;
    public GameObject Parent;
    private Move _Move;
    public float CastSpeed = 70.0f;
    public float Dir = 0.5f;
    public PhotonView PV;
    public GameObject pu2;
    public GameObject pu3;
    public bool a=false;
    public GameObject Water;
    public GameObject hit;
    public float bss=1000f;
    public GameObject other1;
    private void OnEnable()
    {
        transform.SetParent(null);
        StartCoroutine("DirCheck");

    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        _Move = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();

        if(CompareTag("Bullet"))
        {
            bss = 1000f;
        }
        else if(CompareTag("SpeedBullet"))
        {
            bss = 2000f;
        }
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {


        if (_BulletMode == BulletMode.Grenade)
            transform.Translate(Vector3.left * Time.deltaTime * CastSpeed);
        else
            transform.Translate(Vector3.forward * Time.deltaTime * CastSpeed);
    }

    void FixedUpdate2()
    {
    }

    IEnumerator DirCheck()
    {
        if (_BulletMode == BulletMode.Grenade)
            StopCoroutine("DirCheck");

        yield return new WaitForSeconds(Dir);
        PV.RPC("ActiveOff", RpcTarget.All);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground" || other.tag == "Wall" || other.tag == "Fance")
        {
            if (_BulletMode == BulletMode.Grenade)
                return;

            PV.RPC("ActiveOff", RpcTarget.All);
            HitEffect(transform.position.x, transform.position.y, transform.position.z);

            if (other.tag == "Fance")
                other.GetComponent<FenceObj>().DestroyRPC();
        }

        if(other.tag == "Player")
        {
            if (_BulletMode == BulletMode.Grenade)
                return;

            other.GetComponent<BackMove>().PV.RPC("BackRPC", RpcTarget.All,
                transform.position.x, 
                transform.position.y, 
                transform.position.z,
                bss,
                Parent.GetComponent<Move>().PV.Owner.ToString());
            PV.RPC("ActiveOff", RpcTarget.All);
            //other.GetComponent<BackMove>().ObjMoveback2(this.gameObject, 1000f);
            HitEffect(transform.position.x, transform.position.y, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }

    public void HitEffect(float a, float b, float c)
    {
        if (_BulletMode == BulletMode.Grenade)
            return;

        string effectName = null;

        if (_BulletMode == BulletMode.Attack) effectName = "Hit";
        else if (_BulletMode == BulletMode.Machinegun) effectName = "water_hit";

        if(effectName == "water_hit")
        {
            Water.transform.position = new Vector3(a, b, c);
            Water.GetComponent<ParticleSystem>().Play();
        }
        else if(effectName == "Hit")
        {
            hit.transform.position = new Vector3(a, b, c);
            hit.GetComponent<ParticleSystem>().Play();
        }

    }

    [PunRPC]
    void ActiveOff()
    {
        transform.SetParent(Parent.transform);
        gameObject.SetActive(false);
    }


    public void OFF()
    {
        PV.RPC("ActiveOff", RpcTarget.All);
    }
}
