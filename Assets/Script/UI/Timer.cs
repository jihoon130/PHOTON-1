using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Timer : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    public GameObject StartUiObj, InGameUiObj, StartImage, CountBG;
    public GameObject[] StartCountImage;
    public GameObject[] gm;

    public int Minute { get; set; }
    public int Second { get; set; }
    
    public int ItemMinute { get; set; }


    bool EndCheck=false;
    public Text TimerText;

    private bool isTimer;
    private bool isBGSound;

    // Start ----
    public bool isStart;
    private bool isStartCheck = false;
    private bool isStartImage;
    private int m_nStartCount;
    private float XZ = 2f;
    private float StartX = -1250f;

    private void Awake() => PV = GetComponent<PhotonView>();
    void Start()
    {
        Minute = 0;
        Second = 5;
        m_nStartCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] player2 = GameObject.FindGameObjectsWithTag("Player");
        if (player2.Length == PhotonNetwork.PlayerList.Length)
        {
            if(!isStartCheck)
            {
                GameObject.Find("UISoundManager").GetComponent<RobbySound>().SoundPlayer(3);
                CountBG.SetActive(true);
                GameObjChange(StartUiObj, true);
                InGameUiObj.transform.localPosition = new Vector2(2000, 1000);
                StartCountInit();
                isStartCheck = true;
            }
            StartCountUpdate();
            StartImageUpdate();
        }
    }
    private void StartCountUpdate()
    {
        if (m_nStartCount < 0)
        {
            CountBG.SetActive(false);
            StartImage.SetActive(true);
            isStartImage = true;
            return;
        }

        StartCountImage[m_nStartCount].transform.localScale = new Vector3(XZ, XZ, XZ);

        if (StartCountImage[m_nStartCount].transform.localScale.x > 1 && StartCountImage[m_nStartCount].transform.localScale.y > 1)
        {
            XZ -= 2f * Time.deltaTime;
        }
        else
        {
            StartCountImage[m_nStartCount].SetActive(false);
            m_nStartCount--;
            XZ = 2f;

            if (m_nStartCount > -1)
            {
                GameObject.Find("UISoundManager").GetComponent<RobbySound>().SoundPlayer(3);
                StartCountInit();
            }
                
            else
                GameObject.Find("UISoundManager").GetComponent<RobbySound>().SoundPlayer(7);
        }
    }
    private void StartImageUpdate()
    {
        if (!isStartImage)
            return;
        
        StartImage.transform.localPosition = new Vector2(StartX, 0);

        StartX += 5000 * Time.deltaTime;
        if(StartX > 1250)
        {
            StartImage.SetActive(false);
            StartInit();
            isStartImage = false;
        }
    }
    private void StartCountInit() => StartCountImage[m_nStartCount].SetActive(true);
    private void StartInit()
    {
        if (!isBGSound)
        {
            GameObject.Find("BGSound").GetComponent<AudioSource>().Play();
            isBGSound = true;
        }
        if (!isTimer)
            StartCoroutine("TimeCoroutine");

        Timer1();

        GameObjChange(StartUiObj, false);
        InGameUiObj.transform.localPosition = new Vector2(0, 0);
        isStart = true;
    }
    private void GameObjChange(GameObject g, bool isCheck) => g.SetActive(isCheck);
    IEnumerator TimeCoroutine()
    {
        isTimer = true;
        yield return new WaitForSeconds(1f);

        //TODO: 지훈님 여기연
        if(!EndCheck)
        {
            if (gm.Length != PhotonNetwork.PlayerList.Length)
            {
                gm = GameObject.FindGameObjectsWithTag("Player");
            }
            else
            {
                TimerCheck();
            }
        }
        isTimer = false;
    }

    void TimerCheck()
    {
        if (GameObject.Find("test"))
            return;

        ItemMinute++;
        if (ItemMinute > 6)
            ItemMinute = 0;

        if (Minute <= 0 && Second <= 0)
        {
            Minute = 0;
            Second = 0;
            GameObject.Find("UISoundManager").GetComponent<RobbySound>().SoundPlayer(0);
            GameObject.Find("ScoreManager").GetComponent<ScoreManager>().EndScore();
            InGameUiObj.SetActive(false);
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
