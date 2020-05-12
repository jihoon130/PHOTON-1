using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LobbyNetwork : MonoBehaviourPunCallbacks, IPunObservable
{
    public string gameVersion = "1.0";
    PhotonView pv;
    string EnemyNickname;
    public GameObject[] Player;
    public bool[] Ready;
    public int ReadyCount=0;
    public GameObject ReadyButton;
    bool g=false;
    public GameObject[] Spot;
    public Sprite[] images;
    // Start is called before the first frame update
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    private void Start()
    {
        OnLogin();
    }
    // Update is called once per frame
    void Update()
    {
        if (g)
          return;

        if (PhotonNetwork.IsMasterClient)
        {
            ReadyButton.GetComponent<Image>().sprite = images[1];
        }
        else
        {
            ReadyButton.GetComponent<Image>().sprite = images[0];
        }


        if (ReadyButton.GetComponent<Image>().sprite.name == "UI_image_start" && PhotonNetwork.PlayerList.Length <= 1)
        {
            ReadyButton.GetComponent<Button>().enabled = false;
        }

        if (ReadyButton.GetComponent<Image>().sprite.name == "UI_image_start")//&& ReadyCount >= PhotonNetwork.PlayerList.Length-1) // 2   빌드시 수정
        {
            ReadyButton.GetComponent<Button>().enabled = true;
            ReadyButton.GetComponent<Image>().color = new Color(255, 255, 255);
            ReadyButton.GetComponent<Button>().onClick.AddListener(() => CheckReady());
        }


        OpenR();


        GameObject[] taggedEnemys = GameObject.FindGameObjectsWithTag("Player1");

        for (int i = 0; i < taggedEnemys.Length; i++)
        {
            taggedEnemys[i].transform.position = Spot[i].transform.position;

            if(taggedEnemys[i].GetComponent<LobbyPlayer>().pv.Owner.IsMasterClient)
            {
                taggedEnemys[i].GetComponent<LobbyPlayer>().Master.SetActive(true);
            }


            if (PhotonNetwork.MasterClient == null)
            {
                PhotonNetwork.SetMasterClient(taggedEnemys[i].GetComponent<LobbyPlayer>().pv.Owner);
            }
        }
        //     pv.RPC("TransRPC", RpcTarget.All);

    }

    void OnLogin()
    {
        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.QuickResends = 3;
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
        PhotonNetwork.Instantiate("LobbyCh", transform.position, Quaternion.identity);


       


    }



    void OpenR()
    {
        Player = GameObject.FindGameObjectsWithTag("Player1");

        for (int i=0;i<Player.Length;i++)
        {
          //  Ready[i] = Player[i].GetComponent<>
            if(Player[i])
            {
                Ready = new bool[Player.Length];
                Ready[i] = Player[i].GetComponent<LobbyPlayer>().Ready;
            }
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //통신을 보내는 
        if (stream.IsWriting)
        {
            stream.SendNext(PhotonNetwork.NickName);
        }
        //클론이 통신을 받는 
        else
        {
            EnemyNickname = (string)stream.ReceiveNext();
        }
    }

    public void CheckReady()
    {
        if(!g)
        {
            PhotonNetwork.LoadLevel("TaScene");
            g = true;
        }
        /* 빌드시 수정
        GameObject[] taggedEnemys = GameObject.FindGameObjectsWithTag("Finish");

        ReadyCount = taggedEnemys.Length;

        if (!g && ReadyCount >= PhotonNetwork.PlayerList.Length-1 && PhotonNetwork.IsMasterClient)
        {
            if (ReadyCount == 0)
                return;

            g = true;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel("TaScene");
        }*/
    }


    void OnPlayerDisconnected()
    {
        PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
    }


}
