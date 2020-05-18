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

        if (PV.IsMine)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                if (!isTimer)
                    StartCoroutine("TimeCoroutine");

                PV.RPC("TimerRPC", RpcTarget.All);
            }
        }
    }

    IEnumerator TimeCoroutine()
    {
        isTimer = true;
        yield return new WaitForSeconds(1f);
        PV.RPC("TimerCheckRPC", RpcTarget.All);
        isTimer = false;
    }

    [PunRPC]
    void TimerCheckRPC()
    {
        if (Minute <= 0 && Second <= 0)
        {
            Minute = 0;
            Second = 0;

            GameObject.Find("ScoreManager").GetComponent<ScoreManager>().PV.RPC("EndScoreRPC", RpcTarget.All);
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

    [PunRPC]
    void TimerRPC()
    {
        if (Second <= 9)
            TimerText.text = Minute.ToString() + " : 0" + Second.ToString();
        else
            TimerText.text = Minute.ToString() + " : " + Second.ToString();
    }
}
