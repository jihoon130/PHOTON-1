using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class StartUI : MonoBehaviour
{
    public GameObject[] spot;
  public  GameObject[] Player;
    PhotonView pv;
    private void Awake()
    {
       pv = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame

    private void LateUpdate()
    {
        pv.RPC("BatchRPC", RpcTarget.MasterClient);
    }
    [PunRPC]
    void BatchRPC()
    {
        Player = GameObject.FindGameObjectsWithTag("Player");

        if(Player.Length==PhotonNetwork.PlayerList.Length)
        {
            for (int i = 0; i < Player.Length; i++)
            {
                if (spot[i])
                {
                    Player[i].transform.position = spot[i].transform.position;
                    spot[i] = null;
                }
            }
            pv.RPC("BatchEndRPC", RpcTarget.All);
        }
    }
    [PunRPC]
    void BatchEndRPC()
    {
        gameObject.SetActive(false);
    }
}
