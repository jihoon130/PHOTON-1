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
        int ab = Random.Range(0,4);

        string PlayerGetName = GameObject.Find("SelectPlayer").GetComponent<SelectPlayer>().CharacterName;

        if (PlayerGetName == "Blue" || PlayerGetName == "Orange")
            PlayerGetName = "Green";

        PhotonNetwork.Instantiate("Player_" + PlayerGetName, Spawn1[ab].transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0;i<ChatT.Length;i++)
        {
            if(ChatT[i].text != "")
            {
                I[i].SetActive(true);
            }
        }

    }

  
}
