using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class CreateCh : MonoBehaviourPunCallbacks
{
    public GameObject[] Spawn1;
    PhotonView pv;
    bool start = false;
    bool play = false;
    float StartT = 0.0f;
  public  Text Daegi; public GameObject timer;
    // Start is called before the first frame update
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-6, 7), 7f, Random.Range(-23, -30)), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {


    }

  
}
