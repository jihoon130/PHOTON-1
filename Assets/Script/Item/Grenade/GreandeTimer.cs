using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;
public class GreandeTimer : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public Rigidbody rb;
    public Vector3 rs;
    float t = 5.0f;
    public GameObject startPos;
    public GameObject Parent;

    private void OnEnable()
    {
        transform.position = startPos.transform.position;
        rs = startPos.GetComponent<Cameratest>().pos;
        Vector3 ros = new Vector3(rs.x, rs.y + 0.3f, rs.z);
        transform.DOMove(ros, 0.3f);
        transform.SetParent(null);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        //StartCoroutine("DestroyTimer");
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(0.5f);
        ObjectDestroys();
    }


    public void ObjectDestroys()
    {
        if (PV.IsMine)
        {
            PhotonNetwork.Instantiate("Grenade_Boom", this.transform.position, Quaternion.identity);
            PV.RPC("OFFRPC", RpcTarget.All);
        }
    }
    [PunRPC]
    void OFFRPC()
    {
        transform.SetParent(Parent.transform);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Wall"))
        {
        transform.DOKill();
        }

        StartCoroutine("DestroyTimer");
    }
}
