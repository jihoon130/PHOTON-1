using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeCh : MonoBehaviour
{
    public GameObject[] Charater;
    public LobbyPlayer Player;
    private SelectPlayer SelectPoint;
    
    void Start()
    {
        SelectPoint = GameObject.Find("SelectPlayer").GetComponent<SelectPlayer>();
    }

  public  void ChangeCharater1()
    {
        SelectPoint.CharacterName = "Blue";

        Charater[0].SetActive(false);
        Charater[1].SetActive(true);
        Charater[2].SetActive(true);
        Charater[3].SetActive(true);

        Player.ChangeCharacter(0);

    }
  public  void ChangeCharater2()
    {
        SelectPoint.CharacterName = "Orange";

        Charater[0].SetActive(true);
        Charater[1].SetActive(false);
        Charater[2].SetActive(true);
        Charater[3].SetActive(true);

        Player.ChangeCharacter(1);
    }
  public  void ChangeCharater3()
    {
        SelectPoint.CharacterName = "Pink";

        Charater[0].SetActive(true);
        Charater[1].SetActive(true);
        Charater[2].SetActive(false);
        Charater[3].SetActive(true);

        Player.ChangeCharacter(2);
    }
   public void ChangeCharater4()
    {
        SelectPoint.CharacterName = "Green";

        Charater[0].SetActive(true);
        Charater[1].SetActive(true);
        Charater[2].SetActive(true);
        Charater[3].SetActive(false);


        Player.ChangeCharacter(3);
    }
}
