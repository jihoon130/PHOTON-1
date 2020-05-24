using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;
using UnityEngine.UI;
public class Move : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView PV;
    public PlayerAni _PlayerAni;
    // ID
    public int PVGetID;
    public int score2;
    public bool isMove;
    public bool OKE;
    public GameObject Piguck;
    public string NickName;
    public string EnemyNickName;
    public float MoveSpeed = 7.0f;
    public float AngleSpeed = 0.1f;
    public float daepoT = 0.0f;
    bool kk=false;
    float TestRpT;
    bool TestRPB = false;
    // Water
    string chat;
    public GameObject Boonsoo;
    public ScoreManager scoreM;
    private Vector3 currPos;
    public GameObject[] Effects;
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
    public float GooT=0.0f;
    // jump
    public bool isGround;
    public bool isJumping;
    public bool isJumpDown;
    private float fJumptime;
    public int score;
    public GameObject RSpawn;
    public GameObject SpawnT;
    public Text SpawnText;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        PV = GetComponent<PhotonView>();
        _PlayerAni = GetComponent<PlayerAni>();

        SpawnT = GameObject.Find("ResetBG").transform.GetChild(0).gameObject;
        

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
            Effects[1].GetComponent<ParticleSystem>().Stop();
            RSpawn = GameObject.Find("RSpawn");
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
                if (!isMove || isDie || GetComponent<Machinegun>().isMachineRay)
                    return;

                fHorizontal = Input.GetAxisRaw("Horizontal");
                fVertical = Input.GetAxisRaw("Vertical");

                if (fVertical < 0)
                {
                    if(GetComponent<Machinegun>().isMachineAttack)
                        MoveSpeed = 2.5f;
                    else
                        MoveSpeed = 4f;
                }
                else
                {
                    if (GetComponent<Machinegun>().isMachineAttack)
                        MoveSpeed = 4f;
                    else
                        MoveSpeed = 7f;
                }

                Vector3 moveDir = (Vector3.forward * fVertical) + (Vector3.right * fHorizontal);
                tr.Translate(moveDir.normalized * MoveSpeed * Time.deltaTime, Space.Self);

               // gameObject.GetComponentInChildren<TextMesh>().text = NickName;

              //  this.GetComponentInChildren<TextMesh>().text = NickName;

        
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
            {
                OKE = true;
               anit += Time.deltaTime;
            }
            else
            {
                OKE = false;
             anit = 0.0f;
            }

            if(GooT >0.0f)
            {
                GooT -= Time.deltaTime;
                transform.Translate(Vector3.forward * Time.deltaTime*10);
                isMove = false;
                _PlayerAni._State = State.Dash;
            }
            else
            {
                kk = false;
                isMove = true;
            }

            if(TestRpT>0.0f)
            {
                if (SpawnT)
                {
                    SpawnT.SetActive(true);
                    SpawnT.GetComponentInChildren<Text>().text = TestRpT.ToString("N1") + " 초 뒤에 부활합니다.";
                }
                TestRpT -= Time.deltaTime;
            }
            if(TestRpT<=0.0f && isDie)
            {
                ResetPos();
            }

            if(anit>=0.7f)
            {
                DownR();
                _PlayerAni._State = State.IdleRun;
            }

            if (fHorizontal != 0.0f || fVertical != 0.0f && isGround)
            {
                Effects[0].GetComponent<ParticleSystem>().Play();
            }
            else 
            {
                Effects[0].GetComponent<ParticleSystem>().Stop();
            }

            if (OKE)
            {
                RaycastHit hit;
                Vector3 pos = transform.position;
                Vector3 dir = -transform.up;


                if (Physics.Raycast(pos, dir, out hit, 1f))
                {
                    if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Wall"))
                    {
                        DownR();
                        _PlayerAni._State = State.IdleRun;
                    }
                }
            }
            if (Piguck)
                StartCoroutine("DestroyPiguck");
            else
                StopCoroutine("DestroyPiguck");


            if (isDie && RSpawn.GetComponent<Respawn>().f == false)
            {
                RSpawn.GetComponent<Respawn>().a = false;
                rb.velocity = Vector3.zero;
            }

            if (!kk&&Input.GetKeyDown(KeyCode.LeftShift) && isGround)
            {
                kk = true;
                GooT += 0.5f;
            }

            if (daepoT <= 20.0f)
                daepoT += Time.deltaTime;


            if (StopT <= 0.0f)
            {
                if (!isMove)
                    return;


                if (!fl && Input.GetKeyDown(KeyCode.Space) && isGround)
                {
                    _PlayerAni._State = State.Jump_Start;
                    flRP();
                    fJumptime = 0;
                    StartCoroutine("Fade");
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
                        if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Wall"))
                        {
                            DownR();
                        }
                    }
                }

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
        yield return new WaitForSeconds(10f);
        Piguck = null;
    }

    public void PhoenixTimer()
    {
        isPhoenix = true;
        StartCoroutine("Phoenix");
    }

    public void ResetPos()
    {
        if (Piguck)
        {
            PV.RPC("SendMsgRPC", RpcTarget.AllBuffered, Piguck.GetComponent<Move>().PV.Owner.NickName.ToString());
        }

        if (GetComponent<Create>()._BulletMake == BulletMake.Machinegun)
        {
            GetComponent<Machinegun>().MachineDeleteReset();
        }

        rb.velocity = Vector3.zero;

        transform.localPosition = RSpawn.transform.position;
        RSpawn.GetComponent<Respawn>().RePosition();

        SpawnT.SetActive(false);
        TestRPB = false;
        isDie = false;
    }

    [PunRPC]
    void SendMsgRPC(string _msg)
    {
        Piguck.GetComponent<Move>().score += 10;
        Piguck = null;

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



    public void MoveTrue() => isMove = true;
    public void MoveFalse() => isMove = false;

    public void DownR()
    {
        fHorizontal = 0;
        fVertical = 0;
        Effects[1].GetComponent<ParticleSystem>().Play();
        _PlayerAni._State = State.Jump_End;
        isJumping = false;
        isJumpDown = false;
        fl = false;
        isGround = true;
    }

    public void DieTrue()
    {
        if (TestRPB)
            return;
        isDie = true;
        TestRpT += 3.0f;
        TestRPB = true;
    }
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(.5f);
        isJumping = true;
    }

    private void flRP() => fl = true;
}
