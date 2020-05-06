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

        if(ReadyButton.GetComponentInChildren<Text>().text=="Start" && PhotonNetwork.PlayerList.Length <= 1)
        {
            ReadyButton.GetComponent<Button>().enabled = false;
            ReadyButton.GetComponent<Image>().color = new Color(0, 0, 0);
        }

        if (ReadyButton.GetComponentInChildren<Text>().text == "Start" && PhotonNetwork.PlayerList.Length >= 3) // 2
        {
            ReadyButton.GetComponent<Button>().enabled = true;
            ReadyButton.GetComponent<Image>().color = new Color(255, 255, 255);
            ReadyCount = 0;
            ReadyButton.GetComponent<Button>().onClick.AddListener(() => CheckReady());
        }


        OpenR();
    }

    void OnLogin()
    {
        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
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
        PhotonNetwork.Instantiate("333", transform.position, Quaternion.identity);

        if(PhotonNetwork.IsMasterClient)
        {
            ReadyButton.GetComponentInChildren<Text>().text = "Start";
        }
        else
        {
            ReadyButton.GetComponentInChildren<Text>().text = "Ready";
        }
    }

    void OpenR()
    {
        Player = GameObject.FindGameObjectsWithTag("Follow");

        for (int i=0;i<Player.Length;i++)
        {
          //  Ready[i] = Player[i].GetComponent<>
            if(Player[i])
            {
                Ready = new bool[Player.Length];
                Ready[i] = Player[i].GetComponent<LobbyPlayer>().Ready;
                GameObject.Find(i.ToString()).GetComponentInChildren<Text>().text = Player[i].GetComponent<PhotonView>().Owner.ToString();
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
        for(int i=0;i<Player.Length;i++)
        {
            if(Ready[i])
            {
                ReadyCount++;
            }
        }

        if(!g && ReadyCount >= Player.Length-1 && PhotonNetwork.IsMasterClient)
        {
            g = true;
            PhotonNetwork.LoadLevel("TaScene");
        }
    }
}
