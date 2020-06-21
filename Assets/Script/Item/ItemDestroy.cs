using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemDestroy : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    private void Awake() => PV = GetComponent<PhotonView>();

    private void Update()
    {
        if (transform.position.y < 2.8f)
            PV.RPC("DestroyRPC", RpcTarget.All);
    }
    [PunRPC]
    public void DestroyRPC() => Destroy(this.gameObject);
}
