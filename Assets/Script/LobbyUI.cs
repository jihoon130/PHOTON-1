using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyUI : MonoBehaviour
{
    public GameObject CreateRoom;
    public GameObject Setting1;
    public GameObject ExitGame;
    public GameObject[] Roomcharacter;
    public Image BackGround;
    public Sprite[] BackGroundColor;
    public GameObject Hyogwa;
    public GameObject[] Hyogwa1;
    public GameObject Baegung;
    public GameObject[] Baegung1;
    public Sprite[] Hyogwa2;
    public GameObject[] Baegung2;
    public GameObject Ranking;
    public GameObject Rank;
    public GameObject Credit;
    public Text Nic;
    LobbyNetwork LbNet;
    bool b=true;
    bool a = true;

    private void Awake()
    {
        LbNet = GameObject.Find("NetworkManager").GetComponent<LobbyNetwork>();
        Nic.text = Photon.Pun.PhotonNetwork.NickName;
    }

    public void CreateRoomOpen()
    {
        if (CreateRoom.activeInHierarchy == false)
        {
            CreateRoom.SetActive(true);
            Setting1.SetActive(false);
            ExitGame.SetActive(false);
        }
        else
        {
            CreateRoom.SetActive(false);
        }

    }
    public void CreateRoomClose()
    {
        CreateRoom.SetActive(false);
    }

    public void SettingOpen()
    {
        if (Setting1.activeInHierarchy == false)
        {
            Setting1.SetActive(true);
            CreateRoom.SetActive(false);
            ExitGame.SetActive(false);
            int i = PlayerPrefs.GetInt("hyogwa");
            if(i==1)
            {
                hyogwa1();
            }
            else
            {
                hyogwa6();
            }
        }
        else
        {
            Setting1.SetActive(false);
        }
    }

    public void ExitOpen()
    {
        if(ExitGame.activeInHierarchy == false)
        {
            ExitGame.SetActive(true);
            Setting1.SetActive(false);
            CreateRoom.SetActive(false);
        }
        else
        {
            ExitGame.SetActive(false);
        }
    }
    public void EndGame()
    {
        Application.Quit();
    }

    public void ExitClose()
    {
        ExitGame.SetActive(false);
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
        Debug.Log("FF");
        BackGround.sprite = BackGroundColor[3];
    }

    public void hyogwa1()
    {
        Hyogwa.transform.position = Hyogwa1[0].transform.position;
        Baegung2[0].GetComponent<Image>().sprite = Hyogwa2[1];
    }
    public void hyogwa2()
    {
        Hyogwa.transform.position = Hyogwa1[1].transform.position;
        Baegung2[0].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void hyogwa3()
    {
        Hyogwa.transform.position = Hyogwa1[2].transform.position;
        Baegung2[0].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void hyogwa4()
    {
        Hyogwa.transform.position = Hyogwa1[3].transform.position;
        Baegung2[0].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void hyogwa5()
    {
        Hyogwa.transform.position = Hyogwa1[4].transform.position;
        Baegung2[0].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void hyogwa6()
    {
        Hyogwa.transform.position = Hyogwa1[5].transform.position;
        Baegung2[0].GetComponent<Image>().sprite = Hyogwa2[0];
    }

    public void RakingOFF()
    {
        Ranking.SetActive(false);
    }

    public void baegung1()
    {
        Baegung.transform.position = Baegung1[0].transform.position;
        Baegung2[1].GetComponent<Image>().sprite = Hyogwa2[1];
    }

    public void baegung2()
    {
        Baegung.transform.position = Baegung1[1].transform.position;
        Baegung2[1].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void baegung3()
    {
        Baegung.transform.position = Baegung1[2].transform.position;
        Baegung2[1].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void baegung4()
    {
        Baegung.transform.position = Baegung1[3].transform.position;
        Baegung2[1].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void baegung5()
    {
        Baegung.transform.position = Baegung1[4].transform.position;
        Baegung2[1].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void baegung6()
    {
        Baegung.transform.position = Baegung1[5].transform.position;
        Baegung2[1].GetComponent<Image>().sprite = Hyogwa2[0];
    }

    public void CreditOn()
    {
        Credit.SetActive(true);
    }

    public void CreditOff()
    {
        Credit.SetActive(false);
    }
    public void hyogwazero()
    {
        if (b)
        {
            b = false;
            Hyogwa.transform.position = Hyogwa1[0].transform.position;
            PlayerPrefs.SetInt("hyogwa", 1);
            Baegung2[0].GetComponent<Image>().sprite = Hyogwa2[1];
        }
        else
        {
            b = true;
            Hyogwa.transform.position = Hyogwa1[5].transform.position;
            PlayerPrefs.SetInt("hyogwa", 0);
            Baegung2[0].GetComponent<Image>().sprite = Hyogwa2[0];
        }
    }

    public void RankingON()
    {
        Rank.SetActive(true);
        Setting1.SetActive(false);
        CreateRoom.SetActive(false);
        ExitGame.SetActive(false);
    }

    public void baggungzero()
    {
        if (a)
        {
            a = false;
            Baegung.transform.position = Baegung1[0].transform.position;
            Baegung2[1].GetComponent<Image>().sprite = Hyogwa2[1];
        }
        else
        {
            a = true;
            Baegung.transform.position = Baegung1[5].transform.position;
            Baegung2[1].GetComponent<Image>().sprite = Hyogwa2[0];
        }
    }
}
