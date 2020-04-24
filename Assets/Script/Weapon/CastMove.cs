using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CastMove : MonoBehaviourPunCallbacks
{
    public float CastSpeed = 50.0f;
    public PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * CastSpeed);
    }

    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);
}
