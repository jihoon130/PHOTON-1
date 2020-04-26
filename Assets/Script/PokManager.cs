using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class PokManager : MonoBehaviourPunCallbacks
{
    float CTime = 0.0f;
    public PhotonView PV;
    bool OK=false;
    public Text CooltimeT;
    // Start is called before the first frame update
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        CooltimeT = GameObject.Find("PokCool").GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
            return;


        CooltimeT.text = "지뢰 : " + OK.ToString();
        if(CTime<=15.0f)
        {
            CTime += Time.deltaTime;
        }
        else
        {
            OK = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            if(PV.IsMine && Input.GetKeyDown(KeyCode.C) && OK)
            {
                ReSetOK();
                PhotonNetwork.Instantiate("Pok", new Vector3(transform.position.x,transform.position.y - 0.25f ,transform.position.z), Quaternion.identity);
            }
        }
    }

    void ReSetOK()
    {
        CTime = 0.0f;
        OK = false;
    }
}
