using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HIT : MonoBehaviourPunCallbacks
{
    private PhotonView PV;
    private float ftime;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        ftime += Time.deltaTime;
        if(ftime > 2.5f)
        {
            Destroy(gameObject);
            ftime = 0.0f;
        }
    }

}
