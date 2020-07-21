using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class LobbyPlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    public  PhotonView pv;
    [Header("캐릭터 세부 구성")]
    public  Button bt;
    public bool Ready=false;
    public GameObject Master;
    public Text NickName;
    public GameObject Rd;
    private Rigidbody rb;
    public GameObject Me;
    public Sprite[] sprites;
    public GameObject Bonche;
    [Header("닉변경")]
    public GameObject NickC1;
    public InputField Nick2;
    public GameObject Nick3;
    [Header("통신용")]
    public int MyCharacter =0;
    public int EnemyCharacter = 0;
    private string MyNickName;
    private string EnemyNickName;
    // Start is called before the first frame update
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        bt = GameObject.Find("Ready").GetComponent<Button>();
        transform.SetParent(GameObject.Find("Canvas").transform, true);
        MyNickName = PhotonNetwork.NickName;
        SetMyCharater();
    }
    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            NickName.text = PhotonNetwork.NickName;
            if (pv.Owner.IsMasterClient)
            {
                Ready = false;
                Rd.SetActive(false);
                bt.onClick.RemoveListener(() => OKReady());
            }
        }
        else
        {
            Bonche.GetComponent<Image>().sprite = sprites[EnemyCharacter];
            NickName.text = EnemyNickName;
        }
    }

    public void SetMyCharater()
    {
        Me.SetActive(true);
        Nick3.SetActive(true);
        Bonche.GetComponent<Image>().sprite = sprites[MyCharacter];
        bt.onClick.AddListener(() => OKReady());
        GameObject.Find("Mod").GetComponent<ChangeCh>().Player = this;
    }

    public void OKReady()
    {
         pv.RPC("ReadyRPC", RpcTarget.AllBuffered);
    }

    public void ChangeCharacter(int ChangeCount)
    {
        MyCharacter = ChangeCount;
        Bonche.GetComponent<Image>().sprite = sprites[ChangeCount];
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //통신을 보내는 
        if (stream.IsWriting)
        {
            stream.SendNext(MyCharacter);
            stream.SendNext(MyNickName);
        }
        //클론이 통신을 받는 
        else
        {
            EnemyCharacter = (int)stream.ReceiveNext();
            EnemyNickName = (string)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void ReadyRPC()
    {

        if (!Ready)
        {
            Ready = true;
            Rd.SetActive(true);
        }
        else
        {
            Ready = false;
            Rd.SetActive(false);
        }
    }

    public void ChangeNick()
    {
        if (Ready)
            return;

        NickC1 = GameObject.Find("Canvas").GetComponent<ChaningNick>().NickC1;
        NickC1.SetActive(true);
    }



}
