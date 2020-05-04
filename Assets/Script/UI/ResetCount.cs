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



    private void Awake() => PV = GetComponent<PhotonView>();

    void Start()
    {
    }

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null && !isFind)
        {
            _Move = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
            Count = 3;
            isFind = true;
        }
        
        if (isFind)
        {
            if (_Move.isDie)
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
                        _Move.PV.RPC("ResetPosRPC", RpcTarget.AllBuffered);
                        time = 0;
                    }
                    time = 0;
                }
            }
        }
    }
}
