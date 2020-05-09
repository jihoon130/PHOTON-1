using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeCh : MonoBehaviour
{
    public GameObject[] Charater;
    public GameObject sprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public  void ChangeCharater1()
    {
                sprite.transform.position = Charater[0].transform.position;
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
        sprite.transform.position = Charater[1].transform.position;
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
        sprite.transform.position = Charater[2].transform.position;
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
        sprite.transform.position = Charater[3].transform.position;
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
