using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;
public class GreandeTimer : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
public    Rigidbody rb;
    public Vector3 rs;
    bool a = false;
    float t = 5.0f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        StartCoroutine("DestroyTimer");
    }

    private void Update()
    {

        if (rs.x != 0f && !a)
        {
            Vector3 ros = new Vector3(rs.x, rs.y + 0.2f, rs.z);

            transform.DOMove(ros, 0.5f);
         // transform.position = new Vector3(rs.x,rs.y,rs.z);
            a=true;
        }


    }


    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(3f);
        ObjectDestroys();
    }

    public void ObjectDestroys()
    {
        PhotonNetwork.Instantiate("Grenade_Boom", this.transform.position, Quaternion.identity);
        GetComponent<ItemDestroy>().PV.RPC("DestroyRPC", Photon.Pun.RpcTarget.All);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            transform.position = transform.position;
        }
    }
}
