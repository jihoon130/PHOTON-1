using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Text;
using System;

public class ScoreManager : MonoBehaviourPunCallbacks
{


    public Move[] SusoonJung;
    private Move TempObject;


    public PhotonView PV;
    public GameObject[] ScoreUI;
    public GameObject[] EndSco;
    GameObject[] taggedPlayer;
    public Text[] EndSocreText;
    public GameObject End2;
    public GameObject[] ScoreT;
    public GameObject[] RankG;
    // Start is called before the first frame update


    void Start()
    {
        SusoonJung = new Move[PhotonNetwork.PlayerList.Length];
        InvokeRepeating("SetPlayer", 0.0f, 0.1f);

        for (int j = PhotonNetwork.PlayerList.Length; j < 4; j++)
        {
            RankG[j].SetActive(false);
        }

        PV = GetComponent<PhotonView>();



    }
    public void SetPlayer()
    {
        if (taggedPlayer?.Length == PhotonNetwork.PlayerList.Length)
        {
            UpdateScore();
            return;
        }
       taggedPlayer = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < taggedPlayer.Length; i++)
        {
            SusoonJung[i] = taggedPlayer[i].GetComponent<Move>();
        }
    }
    public void EndScore()
    {
        End2.SetActive(true);
        StartCoroutine("EndS");
    }
    IEnumerator EndS()
    {
        foreach(GameObject gameObject in taggedPlayer)
        {
            gameObject.GetComponent<Move>().GameEndok = true;
        }

        yield return new WaitForSeconds(1f);
        //PlayerDB db = GameObject.Find("PlayerDB").GetComponent<PlayerDB>();
        //for(int i=0;i<SusoonJung.Length;i++)
        //{
        //    if(SusoonJung[i].GetComponent<Move>().PV.IsMine)
        //    {
        //        switch(i)
        //        {
        //            case 0:
        //                db.UpdateDB(db.ip, db.UserScore + 20);
        //                EndSco[7].GetComponent<Text>().text =  "점수 : "+(db.UserScore + 20) + "(+20)";
        //                break;
        //            case 1:
        //                db.UpdateDB(db.ip, db.UserScore + 10);
        //                EndSco[7].GetComponent<Text>().text = "점수 : " + (db.UserScore + 10) + "(+10)";
        //                break;
        //            case 2:
        //                db.UpdateDB(db.ip, db.UserScore);
        //                EndSco[7].GetComponent<Text>().text = "점수 : " + (db.UserScore) + "(+0)";
        //                break;
        //            case 3:
        //                db.UpdateDB(db.ip, db.UserScore-10);
        //                EndSco[7].GetComponent<Text>().text = "점수 : " + (db.UserScore - 10) + "(-10)";
        //                break;
        //        }
        //    }
        //}
        GameObject.Find("UISoundManager").GetComponent<RobbySound>().SoundPlayer(1);
        EndSco[0].SetActive(true);
        yield return new WaitForSeconds(2f);
        EndSco[0].SetActive(false);
        for (int i = 1; i <= PhotonNetwork.PlayerList.Length; i++)
        {
            EndSco[i].SetActive(true);
            EndSco[i].GetComponentInChildren<Text>().text = SusoonJung[i-1].PV.Owner.ToString().Substring(4);
            SusoonJung[i - 1].isMove = false;
            EndSco[i].transform.GetChild(1).GetComponent<Text>().text = SusoonJung[i-1].score.ToString();
            
        }
        yield return new WaitForSeconds(2f);
        EndSco[7].SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EndSco[6].SetActive(true);
        EndSco[5].SetActive(true);
       // Destroy(db.gameObject);
        GameObject.Find("UISoundManager").GetComponent<RobbySound>().SoundPlayer(2);
    }


    static void Quicksort(Move[] numbers, int left, int right)
    {
        int l_hold = left, r_hold = right; 
        int pivot = numbers[left].score;      
        Move _pivot = numbers[left];

        while (left < right)
        {
            while ((pivot >= numbers[right].score) && (left < right))
                right--;
            if (left != right)
                numbers[left] = numbers[right];

            while ((pivot <= numbers[left].score) && (left < right))
                left++;
            if (left != right)
            {
                numbers[right] = numbers[left];
                right--; 
            }
        }

        numbers[left] = _pivot;
        pivot = left;
        left = l_hold;
        right = r_hold;

        if (left < pivot)
            Quicksort(numbers, left, pivot - 1);
        if (right > pivot)
            Quicksort(numbers, pivot + 1, right);
    }


    void UpdateScore()
    {
        Quicksort(SusoonJung, 0, SusoonJung.Length - 1);

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (ScoreT[i])
            {
                string[] id = SusoonJung[i].PV.Owner.ToString().Split(Convert.ToChar(39));
                ScoreT[i].GetComponent<Text>().text = id[1] + "\n" + SusoonJung[i].score;
            }
        }
    }


    public void GoTitle()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}
