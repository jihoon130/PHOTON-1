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
        if (transform.position.y < 4.15f)
            PV.RPC("DestroyRPC", RpcTarget.All);
    }
    [PunRPC]
    public void DestroyRPC() => Destroy(this.gameObject);
}
