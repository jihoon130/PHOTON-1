﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class CreateCh : MonoBehaviourPunCallbacks
{
    public GameObject[] Spawn1;
    public Text[] ChatT;
    public GameObject[] I;
    PhotonView pv;
    bool a = false;
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
        
        string PlayerGetName = GameObject.Find("SelectPlayer").GetComponent<SelectPlayer>().CharacterName;

        int ab = Random.Range(0, 3);
                PhotonNetwork.Instantiate("Player_" + PlayerGetName, Spawn1[3].transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0;i<ChatT.Length-1;i++)
        {
            if(ChatT[i].text != "")
            {
                I[i].SetActive(true);
            }
        }



    }

}
