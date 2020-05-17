using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class DestoryPok : MonoBehaviourPunCallbacks
{
    float time=0.0f;
    public bool OK=false;
    public PhotonView PV;
    // Start is called before the first frame update
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(time <=1.5f)
        {
            time += Time.deltaTime;
        }
        else
        {
            OK = true;
        }
    }

    [PunRPC]
    public void DestroyRPC() => Destroy(this.gameObject);
}
