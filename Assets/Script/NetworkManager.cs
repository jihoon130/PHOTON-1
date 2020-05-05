using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    private bool isRoom;
    private int MaxPlayer = 2;
    public Text Daegi;
    public GameObject timer;
    public bool Start = false;
    private bool Play = false;
    public GameObject[] Spawn1;
    private PhotonView pv;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        pv = GetComponent<PhotonView>();

    }

    private void Update()
    {
        if (Play)
            return;

        if(!isRoom)
        {
            Daegi.text = PhotonNetwork.PlayerList.Length + " / " + "4 모두 입장시 게임이 시작됩니다.";
        }

        if (!isRoom && PhotonNetwork.PlayerList.Length == MaxPlayer)
        {
            Daegi.text = "";
            isRoom = true;
        }

        if(isRoom && !Play)
        {
            Start = true;
        }

        if(Start)
        {
            GameObject[] taggedEnemys = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject taggedEnemy in taggedEnemys)
            {
                
                int a;
                while (true)
                {
                    a = Random.Range(0, 4);
                    if (!Spawn1[a])
                    {
                        break;
                    }
                }
                taggedEnemy.GetComponent<Transform>().position = Spawn1[a].transform.position;
                pv.RPC("DestroyRPC", RpcTarget.AllBuffered, a);
                taggedEnemy.GetComponent<Move>().StopT += 10.0f;
            }

            Daegi.text = "10초후 게임이 시작됩니다.";
            StartCoroutine("GameStart");
            Start = false;
            Play = true;
        }

    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    // Update is called once per frame
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }
    public override void OnJoinedRoom()
    {
        Spawn();
    }

    public void Spawn()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-6, 7), 7f, Random.Range(-23, -30)), Quaternion.identity);
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(10f);
        Daegi.text = "";
        timer.SetActive(true);
    }

    [PunRPC]
    void DestroyRPC(int a) => Destroy(Spawn1[a]);
}