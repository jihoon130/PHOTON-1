using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;
using UnityEngine.UI;
public class Move : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView PV;
    private PlayerAni _PlayerAni;

    // ID
    public int PVGetID;
    public int score2;
    public bool isMove;
    public GameObject Piguck;
    public string NickName;
    public string EnemyNickName;
    public float MoveSpeed = 7.0f;
    public float AngleSpeed = 0.1f;
    public float daepoT = 0.0f;
    // Water
    string chat;
    public GameObject Boonsoo;
    public ScoreManager scoreM;
    private Vector3 currPos;
    private Rigidbody rb;
    private Quaternion currRot;
    private Quaternion targetRotation;
    // Move
    private Transform tr;
   public Text[] ChatText;
  public  float anit =0.0f;
    public float fHorizontal;
    public float fVertical;
    bool fl = false;
    public float StopT = 0.0f;
    public bool isPhoenix;
    public bool isDie;

    // jump
    public bool isGround;
    public bool isJumping;
    public bool isJumpDown;
    private float fJumptime;
    public int score;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        PV = GetComponent<PhotonView>();
        _PlayerAni = GetComponent<PlayerAni>();

        ChatText = new Text[3];
        ChatText[0] = GameObject.Find("ChatBox").GetComponent<Text>();
        for (int i = 1; i < ChatText.Length; i++)
        {
            ChatText[i] = GameObject.Find("ChatBox" + i.ToString()).GetComponent<Text>();
        }

        scoreM = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        if (PV.IsMine)
        {
            NickName = PlayerPrefs.GetString("NickName");
        }
    }
    void Start()
    {
        score = 0;
        isMove = true;
        if (PV.IsMine)
        {
            CameraPlayer.I.target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void FixedUpdate()
    {
        //controlled locally일 경우 이동(자기 자신의 캐릭터일 때)
        if (PV.IsMine)
        {
            if (StopT <= 0.0f)
            {
                if (!isMove || isDie)
                    return;

                fHorizontal = Input.GetAxisRaw("Horizontal");
                fVertical = Input.GetAxisRaw("Vertical");

                if (fVertical < 0) MoveSpeed = 4f;
                else MoveSpeed = 7f;

                Vector3 moveDir = (Vector3.forward * fVertical) + (Vector3.right * fHorizontal);
                tr.Translate(moveDir.normalized * MoveSpeed * Time.deltaTime, Space.Self);

                gameObject.GetComponentInChildren<TextMesh>().text = NickName;

                this.GetComponentInChildren<TextMesh>().text = NickName;

        
            }
            else
            {
                StopT -= Time.deltaTime;
            }
        }
        else
        {
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            tr.rotation = Quaternion.Lerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            gameObject.GetComponentInChildren<TextMesh>().text = EnemyNickName;
        }
    }
    private void Update()
    {
        if (PV.IsMine)
        {
            if (_PlayerAni._State == State.Jump_End || _PlayerAni._State == State.Jump_Ing)
                anit += Time.deltaTime;
            else
                anit = 0.0f;

            if(anit >=1.0f)
            {
                DownR();
                _PlayerAni._State = State.IdleRun;
            }
                if (Piguck)
                StartCoroutine("DestroyPiguck");


            if (isDie)
            {
                rb.velocity = Vector3.zero;

            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                _PlayerAni._State = State.Dash;
            }

            if (daepoT <= 20.0f)
                daepoT += Time.deltaTime;


            if (StopT <= 0.0f)
            {
                if (!isMove)
                    return;


                if (!fl && Input.GetKeyDown(KeyCode.Space) && isGround)
                {
                    flRP();
                    fJumptime = 0;
                    StartCoroutine("Fade");
                    _PlayerAni._State = State.Jump_Start;
                    isGround = false;
                }

                if (isJumping)
                {
                    _PlayerAni._State = State.Jump_Ing;

                    rb.velocity = new Vector3(0, -800 * Time.deltaTime, 0);
                    RaycastHit hit;
                    Vector3 pos = transform.position;
                    Vector3 dir = -transform.up;


                    if (Physics.Raycast(pos, dir, out hit, 0.5f))
                    {
                        if (hit.collider.CompareTag("Ground"))
                        {
                            DownR();
                        }
                    }
                }
                //else
                //{
                //    if (_PlayerAni._State == State.Jump_Ing)
                //        _PlayerAni._State = State.Jump_End;
                //}

                if (Input.GetKeyDown(KeyCode.RightShift))
                {
                    PhotonNetwork.Instantiate("333", transform.position, Quaternion.identity);
                }
            }

        }
    }
    private void Jump()
    {
        rb.AddForce(Vector3.up * 6f, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //통신을 보내는 
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
            stream.SendNext(NickName.ToString());
            stream.SendNext(score);
        }
        //클론이 통신을 받는 
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            EnemyNickName = (string)stream.ReceiveNext();
            score2 = (int)stream.ReceiveNext();
        }
    }

    IEnumerator Phoenix()
    {
        yield return new WaitForSeconds(0.3f);
        isPhoenix = false;
    }
    IEnumerator DestroyPiguck()
    {
        yield return new WaitForSeconds(100f);
        Piguck = null;
    }

    public void PhoenixTimer()
    {
        isPhoenix = true;
        StartCoroutine("Phoenix");
    }

    [PunRPC]
    public void ResetPosRPC()
    {
        if (Piguck)
        {
            //scoreM.Score[Piguck.GetComponent<Move>().PV.ViewID / 1000] += 1;
            // Debug.Log(scoreM.Score[Piguck.GetComponent<Move>().PV.ViewID / 1000]);
            Piguck.GetComponent<Move>().score += 1;
            PV.RPC("SendMsgRPC", RpcTarget.AllBuffered, Piguck.GetComponent<Move>().PV.Owner.NickName.ToString());
            //SendMsg();
            Piguck = null;
            // PV.RPC("PlusScoreRPC", RpcTarget.All);
        }

        rb.velocity = Vector3.zero;

        transform.localPosition = new Vector3(0, 10, Random.Range(-3, 3));

        isDie = false;
    }

    [PunRPC]
    void SendMsgRPC(string _msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = _msg + " ---> " +PV.Owner.NickName.ToString();
                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = _msg + " 님이" + PV.Owner.NickName.ToString() + " 을 죽임";
        }
    }

    public void SpeedSetting()
    {
        if (PV.IsMine)
        {
            MoveSpeed = 5;
            StartCoroutine("SpeedTimer");
        }
    }

    IEnumerator SpeedTimer()
    {
        yield return new WaitForSeconds(5f);
        if (PV.IsMine)
            MoveSpeed = 10;
    }


    public void MoveTrue()
    {
        isMove = true;

    }

    public void MoveFalse()
    {
        isMove = false;
    }

    public void DownR()
    {
        fHorizontal = 0;
        fVertical = 0;

        _PlayerAni._State = State.Jump_End;
        isJumping = false;
        isJumpDown = false;
        fl = false;
        isGround = true;
    }

    public void DieTrue()
    {
        isDie = true;
    }
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(.5f);
        isJumping = true;
    }

    void flRP()
    {
        fl = true;
    }


}
