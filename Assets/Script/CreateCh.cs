using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class CreateCh : MonoBehaviourPunCallbacks
{
    public GameObject[] Spawn1;
    PhotonView pv;
    bool start = false;
    bool play = false;
    float StartT = 0.0f;
  public  Text Daegi; public GameObject timer;
    // Start is called before the first frame update
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-6, 7), 7f, Random.Range(-23, -30)), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (play)
            return;

        Daegi.text = "10초후 게임이 시작됩니다.";

        if (!start)
        {
            GameObject[] taggedEnemys = GameObject.FindGameObjectsWithTag("Player");

           foreach (GameObject taggedEnemy in taggedEnemys)
            {
                  taggedEnemy.GetComponent<Move>().StopT += 0.1f;
                        int a;
                         AAA:
                        a = Random.Range(0, 4);
                        if (!Spawn1[a])
                        {
                        goto AAA;
                          }
                taggedEnemy.GetComponent<Transform>().position = Spawn1[a].transform.position;
                pv.RPC("DestroyRPC", RpcTarget.All, a);
            }

            start = false;
            StartCoroutine("GameStart");
            play = true;
        }
    }

    [PunRPC]
    void DestroyRPC(int a) => Destroy(Spawn1[a]);

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(0.1f);
        Daegi.text = "";
        timer.SetActive(true);
    }
}
