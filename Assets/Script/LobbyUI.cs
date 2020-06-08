using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyUI : MonoBehaviour
{
    public GameObject CreateRoom;
    public GameObject[] Roomcharacter;
    public Image BackGround;
    public Sprite[] BackGroundColor;
    LobbyNetwork LbNet;

    private void Awake()
    {
        LbNet = GameObject.Find("NetworkManager").GetComponent<LobbyNetwork>();
    }

    public void CreateRoomOpen()
    {
        CreateRoom.SetActive(true);
    }
    public void CreateRoomClose()
    {
        CreateRoom.SetActive(false);
    }

    public void ChangeC1()
    {
        LbNet.CharacterCount = "1";
        Roomcharacter[2].SetActive(true);
        Roomcharacter[0].SetActive(false);
        Roomcharacter[1].SetActive(false);
        Roomcharacter[3].SetActive(false);
    }
    public void ChangeC2()
    {
        LbNet.CharacterCount = "0";
        Roomcharacter[2].SetActive(false);
        Roomcharacter[0].SetActive(false);
        Roomcharacter[1].SetActive(true);
        Roomcharacter[3].SetActive(false);
    }
    public void ChangeC3()
    {
        LbNet.CharacterCount = "2";
        Roomcharacter[2].SetActive(false);
        Roomcharacter[0].SetActive(false);
        Roomcharacter[1].SetActive(false);
        Roomcharacter[3].SetActive(true);
    }
    public void ChangeC4()
    {
        LbNet.CharacterCount = "3";
        Roomcharacter[2].SetActive(false);
        Roomcharacter[0].SetActive(true);
        Roomcharacter[1].SetActive(false);
        Roomcharacter[3].SetActive(false);
    }

    public void BackGroundC1()
    {
        LbNet.BackGroundColor = "0";
        BackGround.sprite = BackGroundColor[0];
    }
    public void BackGroundC2()
    {
        LbNet.BackGroundColor = "1";
        BackGround.sprite = BackGroundColor[1];
    }
    public void BackGroundC3()
    {
        LbNet.BackGroundColor = "2";
        BackGround.sprite = BackGroundColor[2];
    }
    public void BackGroundC4()
    {
        LbNet.BackGroundColor = "3";
        BackGround.sprite = BackGroundColor[3];
    }
}
