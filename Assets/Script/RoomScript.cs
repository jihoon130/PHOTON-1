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
    public Image BackGround;
    public Sprite[] Sprites;
    public GameObject[] Character;
    public Text roomNameT;
    public Text roomCountT;
    public int a;
    public int b;
    public int Count1;
    public string Count2;
    private void Awake()
    {
        roomDateTxt = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    { }

    public void UpdateInfo()
    {
        BackGround.sprite = Sprites[a];
        Character[b].SetActive(true);
        roomNameT.text = roomName;
        roomCountT.text = Count1.ToString();
        roomDateTxt.text = playerCount + "/" + maxPlayer;

    }
}