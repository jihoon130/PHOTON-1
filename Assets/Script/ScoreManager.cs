using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public int[] Score;
    public int bestScore;
    public string bestNick;
    public int k;
    public Text Scoretext;
    // Start is called before the first frame update
    void Start()
    {
        Score = new int[150];
        InvokeRepeating("ScoreUpdate", 0, 0.1f);
    }
    private void Update()
    {

        for (int i = 0; i < Score.Length; i++)
        {
            if (bestScore <= Score[i])
            {
                bestScore = Score[i];
                k = i;
            }
        }

        if(bestNick != "")
        Scoretext.text = "현재 일등 : " + bestNick + " - " + bestScore.ToString();
        else
        Scoretext.text = "현재 내가 일등" + " - " + bestScore.ToString();
    }
    void ScoreUpdate()
    {
        GameObject[] taggedEnemys = GameObject.FindGameObjectsWithTag("Player1");



        foreach (GameObject taggedEnemy in taggedEnemys)
        {
            Score[taggedEnemy.GetComponent<Move>().PV.ViewID/1000] = taggedEnemy.GetComponent<Move>().score;

            if(k>=1 && taggedEnemy.GetComponent<Move>().PV.ViewID == k*1000+1)
            {
                if (taggedEnemy.GetComponent<Move>().EnemyNickName != null)
                    bestNick = taggedEnemy.GetComponent<Move>().EnemyNickName;
                else
                    bestNick = "MY";
            }
        }


       
    }
}
