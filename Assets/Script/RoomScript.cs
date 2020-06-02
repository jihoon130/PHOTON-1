using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class RoomScript : MonoBehaviour
{
    public string roomName = "";
    public int playerCount = 0;
    public int maxPlayer = 0;
    public Text roomDateTxt;

    private void Awake()
    {
        roomDateTxt = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {}

    public void UpdateInfo()
    {

        roomDateTxt.text = string.Format(" {0} [{1}/{2}]", roomName, playerCount.ToString("00"), maxPlayer);

    }
}
