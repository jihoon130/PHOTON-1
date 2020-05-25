using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Timer : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    public int Minute { get; set; }
    public int Second { get; set; }


    bool EndCheck=false;
    public Text TimerText;
    private bool isTimer;

    private void Awake() => PV = GetComponent<PhotonView>();
    void Start()
    {
        Minute = 2;
        Second = 59;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] player2 = GameObject.FindGameObjectsWithTag("Player");
        if (player2.Length == PhotonNetwork.PlayerList.Length)
        {
            if (!isTimer)
                StartCoroutine("TimeCoroutine");

            Timer1();
        }
    }

    IEnumerator TimeCoroutine()
    {
        isTimer = true;
        yield return new WaitForSeconds(1f);
        if(!EndCheck)
        TimerCheck();
        isTimer = false;
    }

    void TimerCheck()
    {
        if (Minute <= 0 && Second <= 0)
        {
            Minute = 0;
            Second = 0;

            GameObject.Find("ScoreManager").GetComponent<ScoreManager>().EndScore();
            EndCheck = true;
            return;
        }

        if (Second > 0)
        {
            Second--;
        } else
        {
            Minute--;
            Second = 59;
        }
    }

    void Timer1()
    {
        if (Second <= 9)
            TimerText.text = Minute.ToString() + " : 0" + Second.ToString();
        else
            TimerText.text = Minute.ToString() + " : " + Second.ToString();
    }

    public bool TimerCheck(int min, int second)
    {
        if (Minute == min && Second == second)
            return true;
        return false;
    }
}
