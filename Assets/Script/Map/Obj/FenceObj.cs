﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FenceObj : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    private void Awake() => PV = GetComponent<PhotonView>();

    [PunRPC]
    public void DestroyRPC() => Destroy(this.gameObject);
}
