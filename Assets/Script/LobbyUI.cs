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
    public AudioSource mainaudio;
    public AudioSource subaudio;
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

            if(PlayerPrefs.HasKey("hyogwa"))
            {
                switch (PlayerPrefs.GetFloat("hyogwa"))
                {
                    case 0:
                        hyogwa1();
                        break;
                    case 0.2f:
                        hyogwa2();
                        break;
                    case 0.4f:
                        hyogwa3();
                        break;
                    case 0.6f:
                        hyogwa4();
                        break;
                    case 0.8f:
                        hyogwa5();
                        break;
                    case 1f:
                        hyogwa6();
                        break;
                }

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
        PlayerPrefs.SetFloat("hyogwa", 0f);
        mainaudio.volume = 0;
        Hyogwa.transform.position = Hyogwa1[0].transform.position;
        Baegung2[0].GetComponent<Image>().sprite = Hyogwa2[1];
    }
    public void hyogwa2()
    {
        PlayerPrefs.SetFloat("hyogwa", 0.2f);
        mainaudio.volume = 0.2f;
        Hyogwa.transform.position = Hyogwa1[1].transform.position;
        Baegung2[0].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void hyogwa3()
    {
        PlayerPrefs.SetFloat("hyogwa", 0.4f);
        mainaudio.volume = 0.4f;
        Hyogwa.transform.position = Hyogwa1[2].transform.position;
        Baegung2[0].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void hyogwa4()
    {
        PlayerPrefs.SetFloat("hyogwa", 0.6f);
        mainaudio.volume = 0.6f;
        Hyogwa.transform.position = Hyogwa1[3].transform.position;
        Baegung2[0].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void hyogwa5()
    {
        PlayerPrefs.SetFloat("hyogwa", 0.8f);
        mainaudio.volume = 0.8f;
        Hyogwa.transform.position = Hyogwa1[4].transform.position;
        Baegung2[0].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void hyogwa6()
    {
        PlayerPrefs.SetFloat("hyogwa", 1f);
        mainaudio.volume = 1f;
        Hyogwa.transform.position = Hyogwa1[5].transform.position;
        Baegung2[0].GetComponent<Image>().sprite = Hyogwa2[0];
    }

    public void RakingOFF()
    {
        Ranking.SetActive(false);
    }

    public void baegung1()
    {
        PlayerPrefs.SetFloat("baegung", 0f);
        subaudio.volume = 0f;
        Baegung.transform.position = Baegung1[0].transform.position;
        Baegung2[1].GetComponent<Image>().sprite = Hyogwa2[1];
    }

    public void baegung2()
    {
        PlayerPrefs.SetFloat("baegung", 0.2f);
        subaudio.volume = 0.2f;
        Baegung.transform.position = Baegung1[1].transform.position;
        Baegung2[1].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void baegung3()
    {
        PlayerPrefs.SetFloat("baegung", 0.4f);
        subaudio.volume = 0.4f;
        Baegung.transform.position = Baegung1[2].transform.position;
        Baegung2[1].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void baegung4()
    {
        PlayerPrefs.SetFloat("baegung", 0.6f);
        subaudio.volume = 0.6f;
        Baegung.transform.position = Baegung1[3].transform.position;
        Baegung2[1].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void baegung5()
    {
        PlayerPrefs.SetFloat("baegung", 0.8f);
        subaudio.volume = 0.8f;
        Baegung.transform.position = Baegung1[4].transform.position;
        Baegung2[1].GetComponent<Image>().sprite = Hyogwa2[0];
    }
    public void baegung6()
    {
        PlayerPrefs.SetFloat("baegung", 1f);
        subaudio.volume = 1f;
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
            hyogwa1();
            Baegung2[0].GetComponent<Image>().sprite = Hyogwa2[1];
        }
        else
        {
            b = true;
            Hyogwa.transform.position = Hyogwa1[5].transform.position;
            hyogwa6();
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
            baegung1();
            Baegung.transform.position = Baegung1[0].transform.position;
            Baegung2[1].GetComponent<Image>().sprite = Hyogwa2[1];
        }
        else
        {
            a = true;
            baegung6();
            Baegung.transform.position = Baegung1[5].transform.position;
            Baegung2[1].GetComponent<Image>().sprite = Hyogwa2[0];
        }
    }
}
