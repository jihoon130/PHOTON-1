using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CastMove : MonoBehaviourPunCallbacks
{
    public float CastSpeed = 70.0f;
    public float Dir = 0.5f;

    public PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        StartCoroutine("DirCheck");
    }
    private void Update()
    {
        Vector3 pos = transform.position;
        Vector3 PosA = transform.forward;
        RaycastHit hit;
        if(Physics.Raycast(pos,PosA,out hit,1.0f))
        {
            if(hit.collider.CompareTag("Player"))
            {
               hit.collider.GetComponent<BackMove>().PV.RPC("BackRPC", RpcTarget.AllBuffered, transform.position.x, transform.position.y, transform.position.z);
                PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * CastSpeed);
    }

    IEnumerator DirCheck()
    {
        yield return new WaitForSeconds(Dir);
        PV.RPC("DestroysRPC", RpcTarget.AllBuffered);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag == "Ground")
        {
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            PV.RPC("HitEffectRPC", RpcTarget.AllBuffered, transform.position.x, transform.position.y, transform.position.z);
        }
    }

    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);

    [PunRPC]
    void DestroysRPC()
    {
        if (this.gameObject != null)
            Destroy(gameObject);
    }

    [PunRPC]
    public void HitEffectRPC(float a, float b, float c)
    {
        PhotonNetwork.Instantiate("Hit", new Vector3(a, b, c), Quaternion.Euler(0, 0, 0));
    }
}
