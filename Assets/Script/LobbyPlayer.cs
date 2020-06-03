using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class LobbyPlayer : MonoBehaviourPunCallbacks, IPunObservable
{
  public  PhotonView pv;
    public  Button bt;
    bool d=false;
    public bool Ready=false;
    public GameObject Master;
    public Text NickName;
    public GameObject Rd;
    private Rigidbody rb;
    private Quaternion currRot;
    private Vector3 currPos;
    public GameObject Me;
    public Sprite[] sprites;
    public GameObject Bonche;
    public GameObject NickC1;
    public InputField Nick2;
    public GameObject Nick3;
   public int a=0;
    public int b = 0;
    private string c;
    private string z;
    // Start is called before the first frame update
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        bt = GameObject.Find("Ready").GetComponent<Button>();
        transform.SetParent(GameObject.Find("Canvas").transform, true);
    }
    // Update is called once per frame
    void Update()
    {
        c = PhotonNetwork.NickName;
        // pv.RPC("MasterRPC", RpcTarget.All);
        if (pv.IsMine)
        {
            Me.SetActive(true);
            Nick3.SetActive(true);
            Bonche.GetComponent<Image>().sprite = sprites[a];
            NickName.text = PhotonNetwork.NickName;

            if (pv.Owner.IsMasterClient)
            {
                Ready = false;
                Rd.SetActive(false);
                return;
            }

            if (!d &&bt.GetComponent<Image>().sprite.name == "UI_image_ready")
            {
                d = true;
                bt.onClick.AddListener(() => OKReady());
            }
            else
            {
                d = true;
                bt.onClick.RemoveListener(() => OKReady());
            }
        }
        else
        {
            Bonche.GetComponent<Image>().sprite = sprites[b];
            NickName.text = z;
            // transform.position = Vector3.Lerp(transform.position, currPos, Time.deltaTime * 10.0f);
            //  transform.rotation = Quaternion.Lerp(transform.rotation, currRot, Time.deltaTime * 10.0f);
        }
    }
    public void OKReady()
    {
         pv.RPC("ReadyRPC", RpcTarget.All);
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //통신을 보내는 
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(a);
            stream.SendNext(c);
        }
        //클론이 통신을 받는 
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            b = (int)stream.ReceiveNext();
            z = (string)stream.ReceiveNext();
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
