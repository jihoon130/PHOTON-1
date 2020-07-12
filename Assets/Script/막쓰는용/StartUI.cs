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
    int a = 0;
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
        if (PhotonNetwork.IsMasterClient)
        {
            Player = GameObject.FindGameObjectsWithTag("Player");

            if (Player.Length == PhotonNetwork.PlayerList.Length)
            {
                for (int i = 0; i < Player.Length; i++)
                {
                    pv.RPC("BatchRPC", RpcTarget.AllBuffered,Player[i].GetComponent<Move>().PV.Owner.ToString(),i);

                }


                pv.RPC("BatchEndRPC", RpcTarget.AllBuffered);
            }
        }
    }
    [PunRPC]
    void BatchRPC(string i,int a)
    {
        Player = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject player in Player)
        {
            if(player.GetComponent<Move>().PV.Owner.ToString()==i)
            {
                player.transform.position = spot[a].transform.position;
            }
        }
    }
    [PunRPC]
    void BatchEndRPC()
    {
        gameObject.SetActive(false);
    }
}
