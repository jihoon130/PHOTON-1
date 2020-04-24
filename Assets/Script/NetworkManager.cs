using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public byte MaxPlayer;
    private string Versions = "1";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        PhotonNetwork.GameVersion = Versions;
        // 즉시 온라인상태
        PhotonNetwork.ConnectUsingSettings(); 
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = MaxPlayer;
        PhotonNetwork.CreateRoom("room", roomOptions);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(0f, 15f), 0f, Random.Range(0f, 15f)), Quaternion.identity);
    }
}
