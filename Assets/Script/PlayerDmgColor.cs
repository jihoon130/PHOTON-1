using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerDmgColor : MonoBehaviourPunCallbacks
{
    private Move _Move;
    public PhotonView PV;
    public Material[] _material;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        _Move = GetComponentInParent<Move>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        PV.RPC("ColorChangeRPC", RpcTarget.All);
    }

    [PunRPC]
    void ColorChangeRPC()
    {
        if (_Move.isPhoenix)
            GetComponent<SkinnedMeshRenderer>().material = _material[1];
        else
            GetComponent<SkinnedMeshRenderer>().material = _material[0];
    }
}
