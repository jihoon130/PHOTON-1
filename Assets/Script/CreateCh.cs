using System.Collections;
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
    public  Text Daegi;
    public GameObject timer;
    // Start is called before the first frame update
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        string PlayerGetName = GameObject.Find("SelectPlayer").GetComponent<SelectPlayer>().CharacterName;

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
