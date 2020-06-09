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
        //   PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-1, 6), 7f, Random.Range(-18, -24)), Quaternion.identity);

        PhotonNetwork.Instantiate("Player", new Vector3(10.87062f, 5.052387f, -16.19272f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {


    }

  
}
