using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;
public class MachinegunBullet : MonoBehaviourPunCallbacks, IPunObservable
{
    private BulletMaster Bullet;
    public GameObject Parent;
    private Move _Move;
    private PhotonView PV;
    public GameObject hit;
    public Vector3 Point;
    public Vector3 currPos;
    private void OnEnable()
    {
        transform.SetParent(null);

        if (PV.IsMine)
            transform.DOMove(Point, Bullet.MathSpeed(transform.position, Point));
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        _Move = Parent.GetComponent<Move>();
        Bullet = new BulletMaster(30f);
    }

    private void Update()
    {
        if (transform.position == Point)
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

        if (other.tag == "Ground" || other.tag == "Wall" || other.tag == "Fance" || other.gameObject.layer == 11)
        {
            if (other.tag == "Fance")
            {
                PhotonNetwork.Instantiate("WoodEffect", this.transform.position, Quaternion.identity);
                other.GetComponent<FenceObj>().DestroyRPC();
            }


            OFF();
            transform.SetParent(Parent.transform);
            this.gameObject.SetActive(false);
        }

        if (other.tag == "Player")
        {
            AimChange();
            other.GetComponent<BackMove>().PV.RPC("BackRPC", RpcTarget.All,
                transform.position.x,
                transform.position.y,
                transform.position.z,
                Bullet.power,
                _Move.PV.Owner.ToString());
            OFF();
            transform.SetParent(Parent.transform);
            _Move.StartCoroutine("AimOFF");
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


    [PunRPC]
    void ActiveOff()
    {
        hit.transform.position = transform.position;
        hit.GetComponent<ParticleSystem>().Play();
        transform.SetParent(Parent.transform);
        this.gameObject.SetActive(false);
    }

    void OFF()
    {
        PV.RPC("ActiveOff", RpcTarget.All);
    }
}
