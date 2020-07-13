using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemDestroy : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public bool isStartDelete;
    public float TimerDelete;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if(isStartDelete)
        {
            Invoke("Destroys", 5f);
        }
    }
    private void Update()
    {
        if (transform.position.y < 5f && !isStartDelete)
        {
            PV.RPC("DestroyRPC", RpcTarget.All);
        }
    }
    private void Destroys() => PV.RPC("DestroyRPC", RpcTarget.All);
    [PunRPC]
    public void DestroyRPC() => gameObject.SetActive(false);
}
