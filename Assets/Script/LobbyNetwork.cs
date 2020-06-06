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
    public int ReadyCount = 0;
    public GameObject ReadyButton;
    bool g = false;
    public GameObject[] Spot;
    public Sprite[] images;
    public GameObject[] Panels;
    public InputField RoomName;
    public GameObject room;
    public Transform gridTr;
    public InputField ifSnedMsg;
    public ScrollRect sc_rect;
    public Text msglist;
    // Start is called before the first frame update
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        OnLogin();
    }
    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {



        if (g && !PhotonNetwork.InRoom)
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

        if (ReadyButton.GetComponent<Image>().sprite.name == "UI_image_start" && PhotonNetwork.PlayerList.Length >= 1) // 2   빌드시 수정
        {
            ReadyButton.GetComponent<Button>().enabled = true;
            ReadyButton.GetComponent<Image>().color = new Color(255, 255, 255);
            ReadyButton.GetComponent<Button>().onClick.AddListener(() => CheckReady());
        }

        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnSendChatMsg();
        }

        // OpenR();


        GameObject[] taggedEnemys = GameObject.FindGameObjectsWithTag("Player1");

        for (int i = 0; i < taggedEnemys.Length; i++)
        {
            taggedEnemys[i].transform.position = Spot[i].transform.position;

            if (taggedEnemys[i].GetComponent<LobbyPlayer>().pv.Owner.IsMasterClient)
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
        if (PhotonNetwork.IsConnected == false)
            PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.QuickResends = 3;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        //   PhotonNetwork.JoinRandomRoom();
    }
    // Update is called once per frame
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }
    public override void OnJoinedRoom()
    {
        Panels[0].SetActive(false);
        Panels[1].SetActive(true);
        PhotonNetwork.Instantiate("LobbyCh", transform.position, Quaternion.identity);

    }

    public void Test1()
    {
        RoomOptions RO = new RoomOptions();
        RO.MaxPlayers = 4;
        RO.IsOpen = true;
        RO.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom(RoomName.text, RO, TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Room"))
        {
            Destroy(obj);
        }
        foreach (RoomInfo roomInfo in roomList)
        {
            GameObject _room = Instantiate(room, gridTr);
            RoomScript roomDate = _room.GetComponent<RoomScript>();
            roomDate.roomName = roomInfo.Name;
            roomDate.maxPlayer = roomInfo.MaxPlayers;
            roomDate.playerCount = roomInfo.PlayerCount;
            roomDate.UpdateInfo();
            roomDate.GetComponent<Button>().onClick.AddListener
                (
                delegate
                {
                    OnClickRoom(roomDate.roomName);
                }
                );
        }
    }

    void OnClickRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName, null);
    }
    void OpenR()
    {
        Player = GameObject.FindGameObjectsWithTag("Player1");

        for (int i = 0; i < Player.Length; i++)
        {
            //  Ready[i] = Player[i].GetComponent<>
            if (Player[i])
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
        if (!g && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("TaScene");
            g = true;
        }

        //GameObject[] taggedEnemys = GameObject.FindGameObjectsWithTag("Finish");
        //
        //ReadyCount = taggedEnemys.Length;
        //
        //if (!g && ReadyCount >= PhotonNetwork.PlayerList.Length - 1 && PhotonNetwork.IsMasterClient)
        //{
        //    if (ReadyCount == 0)
        //        return;
        //
        //    g = true;
        //    PhotonNetwork.CurrentRoom.IsOpen = false;
        //    PhotonNetwork.LoadLevel("TaScene");
        //}
    }

    private void OnDisconnectedFromServer()
    {
        PhotonNetwork.ReconnectAndRejoin();
    }

    void OnPlayerDisconnected()
    {
        PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
    }

    private void OnFailedToConnect()
    {
        PhotonNetwork.ReconnectAndRejoin();
    }

    private void OnFailedToConnectToMasterServer()
    {
        PhotonNetwork.ReconnectAndRejoin();
    }

    public void OnSendChatMsg()
    {
        if (ifSnedMsg.text != "")
        {
            string msg = string.Format("[{0}] {1}", PhotonNetwork.LocalPlayer.NickName, ifSnedMsg.text);
            ifSnedMsg.text = "";
            ifSnedMsg.ActivateInputField();
            pv.RPC("ReceiveMsgRPC", RpcTarget.OthersBuffered, msg);
            ReceiveMsg(msg);
        }

    }

    [PunRPC]
    void ReceiveMsgRPC(string msg)
    {
        msglist.text += msg + "\n";
        sc_rect.verticalNormalizedPosition = 0.0f;
    }

    void ReceiveMsg(string msg)
    {
        msglist.text += msg + "\n";
        sc_rect.verticalNormalizedPosition = 0.0f;
    }
    void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            PhotonNetwork.ReconnectAndRejoin();
        }
    }

}