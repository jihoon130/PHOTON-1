using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class ScoreManager : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    public GameObject[] ScoreUI;

    public int[] Score;
    public string[] NickName;
    private string SaveNick;
    public GameObject[] EndSco;
    public int bestScore;
    public string bestNick;
    public int k;
    public Text[] EndSocreText;
    public GameObject End2;
    public GameObject[] SusoonJung;
    public GameObject TempObject;
    public GameObject[] ScoreT;
    public GameObject[] RankG;
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
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            RankG[i].SetActive(true);
            for (int j=PhotonNetwork.PlayerList.Length; j<4;j++)
            {
                RankG[j].SetActive(false);
            }
        }
        UpdateScore();
        PV.RPC("UpdateScoreRPC", RpcTarget.All);
    }

    public void EndScore()
    {
        End2.SetActive(true);
        StartCoroutine("EndS");
    }

    IEnumerator EndS()
    {
        yield return new WaitForSeconds(1f);
        EndSco[0].SetActive(true);
        yield return new WaitForSeconds(2f);
        EndSco[0].SetActive(false);
        for (int i = 1; i <= PhotonNetwork.PlayerList.Length; i++)
        {
            EndSco[i].SetActive(true);
            EndSco[i].GetComponentInChildren<Text>().text = SusoonJung[i-1].GetComponent<Move>().PV.Owner.ToString().Substring(4);
            SusoonJung[i - 1].GetComponent<Move>().isMove = false;
            EndSco[i].transform.GetChild(1).GetComponent<Text>().text = Score[SusoonJung[i-1].GetComponent<Move>().PV.ViewID / 1000].ToString();
            
        }
        yield return new WaitForSeconds(2f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EndSco[6].SetActive(true);
        EndSco[5].SetActive(true);
    }


    void ScoreUpdate()
    {
        GameObject[] taggedEnemys = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject taggedEnemy in taggedEnemys)
        {
           Score[taggedEnemy.GetComponent<Move>().PV.ViewID/1000] = taggedEnemy.GetComponent<Move>().score;

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
        if(PhotonNetwork.PlayerList.Length != SusoonJung.Length)
        SusoonJung = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < SusoonJung.Length - 1; i++)
        {

            if (SusoonJung[i].GetComponent<Move>().score < SusoonJung[i + 1].GetComponent<Move>().score)
            {
                TempObject = SusoonJung[i];
                SusoonJung[i] = SusoonJung[i + 1];
                SusoonJung[i + 1] = TempObject;
            }
        }

        for (int i = 0; i < SusoonJung.Length; i++)
        {
            if(ScoreT[i])
            ScoreT[i].GetComponent<Text>().text = SusoonJung[i].GetComponent<Move>().PV.Owner.ToString().Substring(4) + "\n" + Score[SusoonJung[i].GetComponent<Move>().PV.ViewID / 1000];
        }
    }
    void UpdateScore()
    {
        if (PhotonNetwork.PlayerList.Length != SusoonJung.Length)
            SusoonJung = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < SusoonJung.Length - 1; i++)
        {

            if (SusoonJung[i].GetComponent<Move>().score < SusoonJung[i + 1].GetComponent<Move>().score)
            {
                TempObject = SusoonJung[i];
                SusoonJung[i] = SusoonJung[i + 1];
                SusoonJung[i + 1] = TempObject;
            }
        }

        for (int i = 0; i < SusoonJung.Length; i++)
        {
            if (ScoreT[i])
                ScoreT[i].GetComponent<Text>().text = SusoonJung[i].GetComponent<Move>().PV.Owner.ToString().Substring(4) + "\n" + Score[SusoonJung[i].GetComponent<Move>().PV.ViewID / 1000];
        }
    }


    public void GoTitle()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }



    void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            PhotonNetwork.ReconnectAndRejoin();
        }
    }
}
