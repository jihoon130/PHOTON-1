using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class LobbyPlayer : MonoBehaviourPunCallbacks
{
    PhotonView pv;
  public  Button bt;
    bool d=false;
    public bool Ready=false;
    // Start is called before the first frame update
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        bt = GameObject.Find("Ready").GetComponent<Button>();


    }
    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            if (!d &&bt.GetComponentInChildren<Text>().text == "Ready")
            {
                d = true;
                bt.onClick.AddListener(() => OKReady());
            }
        }
    }
    public void OKReady()
    {
        if(pv.IsMine)
        pv.RPC("ReadyRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void ReadyRPC()
    {
        if(!Ready)
        {
            Ready = true;
        }
        else
        {
            Ready = false;
        }
    }
}
