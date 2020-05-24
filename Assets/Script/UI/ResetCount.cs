using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class ResetCount : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    private Move _Move;

    private int Count;
    private float time;
    private bool isFind;
    public GameObject ResetUi;
    public Text ResetCountText;
    GameObject d;


    private void Awake() => PV = GetComponent<PhotonView>();

    void Start()
    {
    }

    void Update()
    {
        d = GameObject.FindGameObjectWithTag("Player");
        if (d.GetComponent<Move>().PV.IsMine && !isFind)
        {
            _Move = d.GetComponent<Move>();
            Count = 3;
            isFind = true;
        }
        
        if (isFind)
        {
            if (_Move.isDie && _Move.PV.IsMine)
            {
                if (GameObject.FindGameObjectWithTag("Player") == null)
                    return;


                ResetUi.SetActive(true);
                ResetCountText.text = Count.ToString();

                time += Time.deltaTime;
                if (time > 1.0f)
                {
                    Count--;
                    if (Count == 0)
                    {
                        ResetUi.SetActive(false);
                        Count = 3;
                        // _Move.ResetPos();
                        time = 0;
                    }
                    time = 0;
                }
            }
        }
    }
}
