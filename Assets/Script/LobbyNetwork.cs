using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LobbyNetwork : MonoBehaviourPunCallbacks, IPunObservable
{
  private string gameVersion = "2.0";
    PhotonView pv;
   string EnemyNickname;
    [Header("방 생성에 필요한것들")]
    public int count1=0;
   public string BackGroundColor;
    public string CharacterCount;
    [Header("로비 안에서 쓰이는 것들")]
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
    public GameObject MakeRoom;
    // Start is called before the first frame update
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        OnLogin();
    }

    // Update is called once per frame
    void Update()
    {
        if (g && !PhotonNetwork.InRoom)
            return;



        if (PhotonNetwork.IsMasterClient)
        {
            ReadyButton.GetComponent<Image>().sprite = images[1];
            ReadyButton.GetComponent<Button>().onClick.AddListener(() => CheckReady());
        }


        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnSendChatMsg();
        }


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

    }

    //초기 셋팅
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
    }

    public override void OnJoinedRoom()
    {
        Panels[0].SetActive(false);
        Panels[1].SetActive(true);
        PhotonNetwork.Instantiate("LobbyCh", transform.position, Quaternion.identity);
    }

    public void InitRoom()
    {
        if (!RoomName)
            return;

        RoomOptions RO = new RoomOptions();
        RO.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        RO.MaxPlayers = 4;
        RO.IsOpen = true;
        RO.IsVisible = true;
        RO.CustomRoomPropertiesForLobby = new string[3];
        RO.CustomRoomPropertiesForLobby[0] = "map";
        RO.CustomRoomPropertiesForLobby[1] = "map1";
        RO.CustomRoomPropertiesForLobby[2] = "map2";
        RO.CustomRoomProperties.Add("map", BackGroundColor);
        RO.CustomRoomProperties.Add("map1", CharacterCount);
        count1 = Random.Range(0, 10000);
        RO.CustomRoomProperties.Add("map2", count1.ToString());

        if(RoomName.text == "")
        {
            RoomName.text = "너만오면고";
        }

        if(MakeRoom.activeInHierarchy == true)
        {
            MakeRoom.SetActive(false);
        }
        
        PhotonNetwork.JoinOrCreateRoom(RoomName.text,  RO, TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
  

        foreach (RoomInfo roomInfo in roomList)
        {
                object ab5;
                roomInfo.CustomProperties.TryGetValue("map2", out ab5);

            if (roomInfo.PlayerCount == 0)
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Room"))
                {
                    if (obj.GetComponent<RoomScript>().roomName == roomInfo.Name)
                    {
                        Destroy(obj);
                    }
                }
                    return;
            }





            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Room"))
            {
                if (obj.GetComponent<RoomScript>().Count2 == ab5.ToString())
                {
                    Destroy(obj);
                }
         }

          

            object ab1;
            object ab2;
            object ab3;
            GameObject _room = Instantiate(room, gridTr);
            RoomScript roomDate = _room.GetComponent<RoomScript>();
            roomInfo.CustomProperties.TryGetValue("map", out ab1);
            roomInfo.CustomProperties.TryGetValue("map1", out ab2);
            roomInfo.CustomProperties.TryGetValue("map2", out ab3);
            if (ab1==null)
            roomDate.a = int.Parse(ab1.ToString());
            if (ab2 == null)
            roomDate.b = int.Parse(ab2.ToString());
            roomDate.Count1 = roomList.IndexOf(roomInfo);
            roomDate.roomName = roomInfo.Name;
            roomDate.maxPlayer = roomInfo.MaxPlayers;
            if(ab3!=null)
            roomDate.Count2 = ab3.ToString();
            roomDate.playerCount = roomInfo.PlayerCount;

            if(roomInfo.IsOpen == false)
            {
                roomDate.GameStart = true;
            }

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

    public void LeftRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinLobby();
        msglist.text = "";
        GameObject[] LobbyC=GameObject.FindGameObjectsWithTag("Player1");
        foreach(GameObject LobbyCC in LobbyC)
        {
            Destroy(LobbyCC);
        }
        Panels[0].SetActive(true);
        Panels[1].SetActive(false);
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
        //if (PhotonNetwork.PlayerList.Length <= 1)
        //    return;


        if (!g && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("TaScene");
            g = true;
        }

        //GameObject[] taggedEnemys = GameObject.FindGameObjectsWithTag("Finish");

        //if (!g && taggedEnemys.Length >= PhotonNetwork.PlayerList.Length && PhotonNetwork.IsMasterClient)
        //{
        //    g = true;
        //    PhotonNetwork.CurrentRoom.IsOpen = false;
        //    PhotonNetwork.LoadLevel("TaScene");
        //}
    }

    private void OnDisconnectedFromServer()
    {
        PhotonNetwork.ReconnectAndRejoin();
    }

    //연결이 끊겼을때 본인을 삭제함.
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

    //보낼 메시지를 설정함.
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


    //다른 사람에게 메시지를 보냄
    [PunRPC]
    void ReceiveMsgRPC(string msg)
    {
        msglist.text += msg + "\n";
        sc_rect.verticalNormalizedPosition = 0.0f;
    }

    //나도 메시지를 늘림
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