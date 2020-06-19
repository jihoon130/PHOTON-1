using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Timer : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    public GameObject StartUiObj, InGameUiObj;

    public int Minute { get; set; }
    public int Second { get; set; }


    bool EndCheck=false;
    public Text TimerText;

    private bool isTimer;
    private bool isBGSound;

    // Start ----
    public bool isStart;
    private bool isStartCheck = false;
    private int m_nStartCount = 3;

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
            //if(!isStartCheck)
            //{
            //    GameObjChange(StartUiObj, true);
            //    StartCoroutine("StartCounting");
            //    isStartCheck = true;
            //}

            if (!isBGSound)
            {
                GameObject.Find("BGSound").GetComponent<AudioSource>().Play();
                isBGSound = true;
            }
            if (!isTimer)
                StartCoroutine("TimeCoroutine");

            Timer1();
        }
    }

    IEnumerator StartCounting()
    {
        while(true)
        {
            if (m_nStartCount < 1)
            {
                GameObjChange(StartUiObj, false);
                GameObjChange(InGameUiObj, true);
                isStart = true;
                StopCoroutine("StartCounting");
            }

            yield return new WaitForSeconds(1f);
            m_nStartCount--;
            Debug.Log(m_nStartCount);
        }
    }

    private void StartCount()
    {
    }

    private void GameObjChange(GameObject g, bool isCheck) => g.SetActive(isCheck);

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
