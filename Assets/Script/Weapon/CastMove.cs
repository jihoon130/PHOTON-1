using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;
public class CastMove : MonoBehaviourPunCallbacks, IPunObservable
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
    public Vector3 sd;
    public Vector3 currPos;
    private void OnEnable()
    {
        transform.SetParent(null);
        if(PV.IsMine)
        transform.DOMove(sd, 0.5f);
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        _Move = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();

        if(CompareTag("Bullet"))
        {
            bss = 1000f;
        }
        else if (CompareTag("SpeedBullet"))
        {
            bss = 2000f;
        }
    }

    private void Update()
    {
       if(transform.position == sd)
        {
            OFF();
        }

        if (!PV.IsMine)
            transform.position = currPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PV.IsMine)
            return;

        if (other.tag == "Ground" || other.tag == "Wall" || other.tag =="Fance" || other.gameObject.layer==11)
        {
            if (_BulletMode == BulletMode.Grenade)
                return;

            HitEffect(transform.position.x, transform.position.y, transform.position.z);

            if (other.tag == "Fance")
                other.GetComponent<FenceObj>().DestroyRPC();

            OFF();
            transform.SetParent(Parent.transform);
            this.gameObject.SetActive(false);
        }

        if(other.tag == "Player")
        {
            if (_BulletMode == BulletMode.Grenade)
                return;
            AimChange();
            other.GetComponent<BackMove>().PV.RPC("BackRPC", RpcTarget.All,
                transform.position.x, 
                transform.position.y, 
                transform.position.z,
                bss,
                Parent.GetComponent<Move>().PV.Owner.ToString());
            OFF();
            //other.GetComponent<BackMove>().ObjMoveback2(this.gameObject, 1000f);
            HitEffect(transform.position.x, transform.position.y, transform.position.z);
            transform.SetParent(Parent.transform);
            Parent.GetComponent<Move>().StartCoroutine("AimOFF");
            this.gameObject.SetActive(false);
        }
    }



    public void AimChange()
    {
        Parent.GetComponent<Create>().sp.GetComponent<Animator>().SetBool("Piguck", true);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //통신을 보내는 
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        //클론이 통신을 받는 
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
        }
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
        HitEffect(transform.position.x, transform.position.y, transform.position.z);
        transform.SetParent(Parent.transform);
        this.gameObject.SetActive(false);
    }

    void OFF()
    {
        PV.RPC("ActiveOff", RpcTarget.All);
    }

}
