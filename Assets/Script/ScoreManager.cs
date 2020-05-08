using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class ScoreManager : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    public GameObject[] ScoreUI;

    public int[] Score;
    public string[] NickName;
    private string SaveNick;

    public int bestScore;
    public string bestNick;
    public int k;
    public Text Scoretext;
    public Text EndSocreText;
    public GameObject[] SusoonJung;
    public GameObject TempObject;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        Score = new int[5];
        NickName = new string[5];
        InvokeRepeating("ScoreUpdate", 0f, 0.1f);
    }
    private void Update()
    {
        PlayerGetScore();
        //Debug.Log(SusoonJung.Length);
        for (int i = 0 ; i < SusoonJung.Length - 1 ; i++)
        {
            //Debug.Log(SusoonJung[i].GetComponent<Move>().score);
            //Debug.Log(SusoonJung[i + 1].GetComponent<Move>().score);
        
            if (SusoonJung[i].GetComponent<Move>().score < SusoonJung[i + 1].GetComponent<Move>().score)
            {
                TempObject = SusoonJung[i];
                SusoonJung[i] = SusoonJung[i + 1];
                SusoonJung[i + 1] = TempObject;
            }
        }

        PV.RPC("UpdateScoreRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void EndScoreRPC()
    {
        ScoreUI[0].SetActive(false);
        ScoreUI[1].SetActive(true);
    }

    void ScoreUpdate()
    {
        GameObject[] taggedEnemys = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject taggedEnemy in taggedEnemys)
        {
          //  Score[taggedEnemy.GetComponent<Move>().PV.ViewID/1000] = taggedEnemy.GetComponent<Move>().score;

            if(k>=1 && taggedEnemy.GetComponent<Move>().PV.ViewID == k*1000+1)
            {
                if (taggedEnemy.GetComponent<Move>().EnemyNickName != null)
                    bestNick = taggedEnemy.GetComponent<Move>().EnemyNickName;
                else
                    bestNick = taggedEnemy.GetComponent<Move>().NickName;
                //bestNick = "MY";
            }
        }
    }

    void PlayerGetScore()
    {
        SusoonJung = GameObject.FindGameObjectsWithTag("Player");
        //for (int i = 0; i <taggedEnemys.Length; i++)
        //{
        //    NickName[i] = taggedEnemys[i].GetComponent<Move>().PV.Owner.ToString();
        //    //if(NickName[i] == null)

        //}

        //foreach ( GameObject g in taggedEnemys)
        //{
        //    Score[g.GetComponent<Move>().PV.ViewID / 1000] = g.GetComponent<Move>().score;
        //}
    }

    void ScroeFindMax()
    {
        for(int x = 1; x < Score.Length - 1; x++)
        {
            for(int y = x + 1; y < Score.Length; y++)
            {
                if(Score[x] < Score[y])
                {
                    bestScore = Score[y];
                    Score[y] = Score[x];
                    Score[x] = bestScore;
                }
            }
        }
        //Debug.Log(SusoonJung.Length);
        //for (int i = 0; i < SusoonJung.Length - 1; i++)
        //{
        //    for (int y = i + 1; y < SusoonJung.Length; y++)
        //    {
        //        if (SusoonJung[i].GetComponent<Move>().score < SusoonJung[y].GetComponent<Move>().score)
        //        {
        //            TempObject = SusoonJung[y];
        //            SusoonJung[y] = SusoonJung[i];
        //            SusoonJung[y] = TempObject;
        //        }
        //    }
        //}
    }
    [PunRPC]
    void UpdateScoreRPC()
    {
        Scoretext.text = "";
        for (int i = 0; i < SusoonJung.Length; i++)
        {
            string Texts = SusoonJung[i].GetComponent<Move>().PV.Owner.ToString();
            Scoretext.text += i + 1 + "등 : " + Texts.Substring(4) + "  점수 : " + Score[SusoonJung[i].GetComponent<Move>().PV.ViewID / 1000] + "\n" + "\n";
        }
    }
}
