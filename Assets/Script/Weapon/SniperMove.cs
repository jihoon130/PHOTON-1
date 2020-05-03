using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SniperMove : MonoBehaviourPunCallbacks
{
    public float CastSpeed = 150.0f;
    public PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        StartCoroutine("DirCheck");
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * CastSpeed);
    }

    IEnumerator DirCheck()
    {
        yield return new WaitForSeconds(1.0f);
        PV.RPC("DestroysRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);

    [PunRPC]
    void DestroysRPC()
    {
        if (this.gameObject != null)
            Destroy(gameObject);
    }
}
