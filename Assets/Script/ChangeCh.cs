using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeCh : MonoBehaviour
{
    public GameObject[] Charater;

    private SelectPlayer SelectPoint;
    
    void Start()
    {
        SelectPoint = GameObject.Find("SelectPlayer").GetComponent<SelectPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public  void ChangeCharater1()
    {
        SelectPoint.CharacterName = "Blue";

                Charater[0].SetActive(false);
                Charater[1].SetActive(true);
                Charater[2].SetActive(true);
                 Charater[3].SetActive(true);
    
        GameObject[] taggedEnemys = GameObject.FindGameObjectsWithTag("Player1");

        for(int i=0;i<taggedEnemys.Length;i++)
        {
            if(taggedEnemys[i].GetComponent<LobbyPlayer>().pv.IsMine)
            {
                taggedEnemys[i].GetComponent<LobbyPlayer>().a = 0;
            }
        }
    }
  public  void ChangeCharater2()
    {
        SelectPoint.CharacterName = "Green";

        Charater[0].SetActive(true);
        Charater[1].SetActive(false);
        Charater[2].SetActive(true);
        Charater[3].SetActive(true);

        GameObject[] taggedEnemys = GameObject.FindGameObjectsWithTag("Player1");

        for (int i = 0; i < taggedEnemys.Length; i++)
        {
            if (taggedEnemys[i].GetComponent<LobbyPlayer>().pv.IsMine)
            {
                taggedEnemys[i].GetComponent<LobbyPlayer>().a = 1;
            }
        }
    }
  public  void ChangeCharater3()
    {
        SelectPoint.CharacterName = "Orange";

        Charater[0].SetActive(true);
        Charater[1].SetActive(true);
        Charater[2].SetActive(false);
        Charater[3].SetActive(true);

        GameObject[] taggedEnemys = GameObject.FindGameObjectsWithTag("Player1");

        for (int i = 0; i < taggedEnemys.Length; i++)
        {
            if (taggedEnemys[i].GetComponent<LobbyPlayer>().pv.IsMine)
            {
                taggedEnemys[i].GetComponent<LobbyPlayer>().a = 2;
            }
        }
    }
   public void ChangeCharater4()
    {
        SelectPoint.CharacterName = "Pink";

        Charater[0].SetActive(true);
        Charater[1].SetActive(true);
        Charater[2].SetActive(true);
        Charater[3].SetActive(false);

        GameObject[] taggedEnemys = GameObject.FindGameObjectsWithTag("Player1");

        for (int i = 0; i < taggedEnemys.Length; i++)
        {
            if (taggedEnemys[i].GetComponent<LobbyPlayer>().pv.IsMine)
            {
                taggedEnemys[i].GetComponent<LobbyPlayer>().a = 3;
            }
        }
    }
}
