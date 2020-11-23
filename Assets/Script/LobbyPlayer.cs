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
    public TextMesh NickName;
    public GameObject Rd;
    private Rigidbody rb;
    public GameObject Me;
    public GameObject[] sprites;
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
    public Animator animator;
    // Start is called before the first frame update
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        bt = GameObject.Find("Ready").GetComponent<Button>();
        MyNickName = PhotonNetwork.NickName;
        if (pv.IsMine)
            SetMyCharater();
        transform.rotation = Quaternion.Euler(new Vector3(0.5f,-170f,-4f));
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            NickName.text = PhotonNetwork.NickName;
            if (pv.Owner.IsMasterClient)
            {
                bt.onClick.RemoveListener(() => OKReady());
            }
            if (Input.GetKeyDown(KeyCode.Y)) animator.SetInteger("Ani", 1);
            if (Input.GetKeyDown(KeyCode.Z)) animator.SetInteger("Ani", 2);
        }
        else
        {
            NickName.text = EnemyNickName;
        }
    }

    public void SetMyCharater()
    {
        //Me.SetActive(true);
        //Nick3.SetActive(true);
        //Bonche.GetComponent<Image>().sprite = sprites[MyCharacter];
        bt.onClick.AddListener(() => OKReady());
        GameObject.Find("Mod").GetComponent<ChangeCh>().Player = this;
    }

    public void OKReady()
    {
        if(pv.IsMine)
         pv.RPC("ReadyRPC", RpcTarget.AllBuffered);
    }

    public void ChangeCharacter(int ChangeCount)
    {
        pv.RPC("ChangeCharacterRPC", RpcTarget.AllBuffered, ChangeCount);
    }

    [PunRPC]
    public void ChangeCharacterRPC(int ChangeCount)
    {
        for(int i = 0; i < sprites.Length;  i++)
            sprites[i].SetActive(false);

        sprites[ChangeCount].SetActive(true);
        animator = sprites[ChangeCount].GetComponent<Animator>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(PhotonNetwork.NickName);
        }
        else
        {
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

    public void DefaultAni()
    {
        animator.SetInteger("Ani", 0);
    }

    public void ChangeNick()
    {
        if (Ready)
            return;

        NickC1 = GameObject.Find("Canvas").GetComponent<ChaningNick>().NickC1;
        NickC1.SetActive(true);
    }



}
