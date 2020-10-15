using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class PlayerSubScript : MonoBehaviourPunCallbacks
{
    public GameObject Die;
    public string st = "";
    public int dt=0;
    public Text txt;
    public float a=0.0f;
    public PhotonView pv;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(a>=0.1f)
        {
            a -= Time.deltaTime;
        }
        else
        {
            Die.SetActive(false);
        }

        if(dt>=1)
        {
            DieOK();
            dt = 0;
        }
    }

    public void DieOK()
    {
        if (!pv.IsMine)
            return;

        Die.SetActive(true);
        txt.text = st + " 님을 빠트렸습니다.+10";
        a = 3.0f;
    }
}
