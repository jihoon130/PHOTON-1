using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Step34 : MonoBehaviour
{
    public GameObject[] chor;
    public PhotonView pv;
    // Start is called before the first frame update
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    void DestroyRPC(int i) => Destroy(chor[i]);
}
