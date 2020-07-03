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


    private void OnEnable()
    {
        transform.position = startPos.transform.position;
        Debug.Log(startPos.GetComponent<Cameratest>().pos);
        rs = startPos.GetComponent<Cameratest>().pos;
        Vector3 ros = new Vector3(rs.x, rs.y + 0.3f, rs.z);
        transform.DOMove(ros, 0.3f);
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
            GetComponent<ItemDestroy>().PV.RPC("DestroyRPC", Photon.Pun.RpcTarget.All);
        }
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
