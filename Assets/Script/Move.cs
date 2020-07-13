using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;
using UnityEngine.UI;
public class Move : MonoBehaviourPunCallbacks, IPunObservable
{
    public string punID;

    public PhotonView PV;
    public PlayerAni _PlayerAni;
    public GameObject camera2;
    // ID
    public int PVGetID;
    public int score2;
    public bool GameEndok=false;
    public Vector3 repo;
    public bool isMove;
    public bool OKE;
    public GameObject Piguck;
    public string Piguck2;
  public  bool dieOk = false;
    public string NickName;
    public string EnemyNickName;
    public float MoveSpeed = 7.0f;
    public float AngleSpeed = 0.1f;
    public float daepoT = 0.0f;
    int ChatCount=0;
    public GameObject[] RespawnG;
    bool kk=false;
    float TestRpT;
    private CapsuleCollider kimsunwoo;
    bool TestRPB = false;
    // Water
    string chat;
    public GameObject Boonsoo;
    public ScoreManager scoreM;
    private Vector3 currPos;
    public GameObject[] Effects;
    private Rigidbody rb;
    public ParticleSystem Balsa;
    private Quaternion currRot;
    private Quaternion targetRotation;
    public GameObject Canvas1;
    // Move
    private Transform tr;
    public Text[] ChatText;
    public  float anit =0.0f;
    public float fHorizontal;
  //  private CreateMesh cm;
    public float fVertical;
    bool fl = false;
    public Transform nickt;
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
    public Create Moogi;
    public Text SpawnText;
    public GameObject kimsunwoo2;
    public GameObject kimsunwoo3;
    public Vector3 kimsunwoo7;
    public bool kimsunwoo4;
    public int Testint=-1;
    // Sound
    AudioSource Audio;
    public AudioClip[] audios;
    public  Queue<string> ChatList = new Queue<string>();
    //  0 - 구르기

    private Timer Timers;
    private BackMove _BackMove;

    public bool isRespawnAttack;

    private void Awake()
    {
        Canvas1 = GameObject.Find("GameCanvas");
        kimsunwoo = GetComponent<CapsuleCollider>();
        //cm = GetComponentInChildren<CreateMesh>();
        camera2 = GameObject.Find("RespawnM");
        Timers = GameObject.Find("TimerManger").GetComponent<Timer>();
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        PV = GetComponent<PhotonView>();
        _PlayerAni = GetComponent<PlayerAni>();
        SpawnT = GameObject.Find("ResetBG").transform.GetChild(0).gameObject;
        Audio = GetComponentInChildren<AudioSource>();
        kimsunwoo2 = GetComponentInChildren<TextMesh>().gameObject;
        _BackMove = GetComponent<BackMove>();


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

        for(int i=0;i<4;i++)
        {
            RespawnG[i] = GameObject.Find("R" + i.ToString());
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
    private void SoundPlayer(int type)
    {
        Audio.clip = audios[type];
        Audio.Play();
    }
    private void FixedUpdate()
    {
        //controlled locally일 경우 이동(자기 자신의 캐릭터일 때)
        if (PV.IsMine)
        {
            if (StopT <= 0.0f && Timers.isStart)
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
            if (Vector3.Distance(tr.position, currPos) >= 20.0f)
            {
                tr.position = currPos;
                tr.rotation = Quaternion.Lerp(tr.rotation, currRot, Time.deltaTime * 100.0f);
            }
            else
            {
                tr.position = new Vector3(tr.position.x,currPos.y,tr.position.z);
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
                tr.rotation = Quaternion.Lerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            }
            gameObject.GetComponentInChildren<TextMesh>().text = EnemyNickName;
        }
    }
    private void Update()
    {
        if (Piguck2 != "")
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject objecte in objects)
            {
                if (Piguck2 == objecte.GetComponent<Move>().PV.Owner.ToString())
                {
                    Piguck = objecte;
                    Piguck2 = null;
                }
            }
        }

        if (PV.IsMine)
        {
            if (!Timers.isStart)
                return;

            if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space))
                return;

          

            if (GooT > 0.0f)
            {
              //  cm.a = true;
                GooT -= Time.deltaTime;
                transform.Translate(new Vector3(Vector3.forward.x, 0f, Vector3.forward.z) * Time.deltaTime * 10);
                isMove = false;
                _PlayerAni._State = State.Dash;
            }
            else
            {
              //  cm.a = false;
                kk = false;
                isMove = true;
            }

            if (TestRpT > 0.0f)
            {
                StopCoroutine("DestroyPiguck");

                if (SpawnT)
                {
                    SpawnT.SetActive(true);
                    SpawnT.GetComponentInChildren<Text>().text = "너무 상심하지마요 상대가 '나'잖아" + "\n" + TestRpT.ToString("N1") + " 초 뒤에 부활합니다.";
                }
                TestRpT -= Time.deltaTime;
            }
            //if (TestRpT <= 0.0f && isDie)
            //{
            //    ResetPos();
            //}


            if(dieOk)
            {
                float time = Canvas1.transform.GetChild(7).gameObject.transform.GetChild(1).gameObject.GetComponent<Step2>().time;
                if (time <= 0.0f)
                {
                    repo = RespawnG[Random.Range(0,4)].transform.position;
                    repo.y = 4.896467f;
                    ResetPos();
                }
            }

            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                GetComponent<Grenade>().DeleteGreade();
                GetComponent<Machinegun>().MachineDeleteReset();
            }


            if (_PlayerAni && fHorizontal == 0.0f && fVertical == 0.0f)
            {
                //kimsunwoo.enabled = false;
                _PlayerAni.Ani.SetLayerWeight(1, 0);
            }
            else
            {
                _PlayerAni.Ani.SetLayerWeight(1, 1);
            }


            if (fHorizontal != 0.0f || fVertical != 0.0f && isGround)
            {
                Effects[0].GetComponent<ParticleSystem>().Play();
            }
            else
            {
                Effects[0].GetComponent<ParticleSystem>().Stop();
            }

            if (Piguck)
                StartCoroutine("DestroyPiguck");
            else
                StopCoroutine("DestroyPiguck");


            if (isDie)
            {
                rb.velocity = Vector3.zero;
            }

            if (!kk&&Input.GetKeyDown(KeyCode.LeftShift) && isGround && !GetComponent<Machinegun>().isMachineRay && !GetComponent<Create>().isReload && !isDie)
            {
                SoundPlayer(0);
                kk = true;
                GooT += 0.5f;
            }

            if (daepoT <= 20.0f)
                daepoT += Time.deltaTime;


            if (StopT <= 0.0f)
            {
                if (!isMove)
                    return;


                if (Input.GetKeyDown(KeyCode.RightShift))
                {
                    PhotonNetwork.Instantiate("333", transform.position, Quaternion.identity);
                }
            }

        }
        else
        {
            if(!kimsunwoo4)
            {
                GameObject[] kimsunwoo5 = GameObject.FindGameObjectsWithTag("Player");

                foreach(GameObject kimsunwoo6 in kimsunwoo5)
                {
                    if (kimsunwoo6.GetComponent<Move>().PV.IsMine)
                    {
                        kimsunwoo3 = kimsunwoo6;
                        kimsunwoo4 = true;
                    }
                }
            }


            kimsunwoo2.transform.rotation = Quaternion.Slerp(kimsunwoo2.transform.rotation,kimsunwoo3.transform.rotation, Time.deltaTime * 50f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!PV.IsMine || GameEndok)
            return;

        if(collision.gameObject.name == "Sea" && !dieOk)
        {
            int a = Random.Range(3, 6);
            Audio.clip = audios[a];
            Audio.Play();

            dieOk = true;
            Canvas1.transform.GetChild(6).gameObject.SetActive(true);
            Canvas1.transform.GetChild(7).gameObject.SetActive(true);
            transform.position = new Vector3(-39.8f, 3.45f, Random.Range(20.0f, 34.0f));
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            camera2.transform.GetChild(4).gameObject.SetActive(true);
        }

        if (collision.gameObject.CompareTag("Ground") && dieOk)
        {

            Canvas1.transform.GetChild(6).gameObject.SetActive(false);
            Canvas1.transform.GetChild(7).gameObject.SetActive(false);
            dieOk = false;
            isDie = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            camera2.transform.GetChild(4).gameObject.SetActive(false);
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
            if (Piguck)
            {
                stream.SendNext(Piguck.GetComponent<Move>().PV.Owner.ToString());
            }
            else
            {
                stream.SendNext("");
            }
        }
        //클론이 통신을 받는 
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            EnemyNickName = (string)stream.ReceiveNext();
            score2 = (int)stream.ReceiveNext();
            Piguck2 = (string)stream.ReceiveNext();
        }
    }

    IEnumerator RespawnShaderOFF()
    {
        SoundPlayer(2);
        yield return new WaitForSeconds(1.0f);
        _BackMove.RespawnUpdate(0f);
    }
    IEnumerator AimOFF()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Create>().sp.GetComponent<Animator>().SetBool("Piguck", false);
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
        if (isPhoenix)
            return;

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
            GetComponent<Machinegun>().MachineDeleteReset();
        else if (GetComponent<Create>()._BulletMake == BulletMake.Grenade)
            GetComponent<Grenade>().DeleteGreade();

        GetComponent<Machinegun>().PlayerDie();
        GetComponent<Grenade>().PlayerDie();

        rb.velocity = Vector3.zero;
        transform.position = repo;
        isPhoenix = false;
        SpawnT.SetActive(false);
        TestRPB = false;
        camera2.transform.GetChild(4).gameObject.SetActive(false);
        isDie = false;

        _BackMove.RespawnUpdate(1f);
        
        StartCoroutine("RespawnShaderOFF");
    }


    [PunRPC]
    void SendMsgRPC(string _msg)
    {
        if (Piguck)
        {
            PlayerSubScript pa = Piguck.GetComponent<PlayerSubScript>();
            pa.st = PV.Owner.ToString().Substring(4);
            pa.dt = pa.GetComponent<Move>().PV.ViewID;
            Piguck.GetComponent<Move>().score += 10;
            Piguck = null;
        }

        ChatList.Enqueue(_msg);

        if(ChatCount ==3)
        {
            ChatCount = 0;
        }


                if(ChatText[ChatCount].GetComponentInParent<KillLog>().deleteT <= 0.1f)
                {
                    ChatText[ChatCount].GetComponentInParent<KillLog>().deleteT = 3.0f;
                    ChatText[ChatCount].text = ChatList.Dequeue();
                    ++ChatCount;
                }
                else
                {
                    ChatText[ChatCount].GetComponent<KillLog>().ResetT();
                    ChatText[ChatCount].GetComponentInParent<KillLog>().deleteT = 3.0f;
                    ChatText[ChatCount].text = ChatList.Dequeue();
                    ++ChatCount;
                }
        //for (int i = 0; i < ChatText.Length; i++)
        //{
        //    if(ChatCount ==  ChatText.Length)
        //    {
        //        ChatCount = 0;
        //    }

        //        ChatText[ChatCount].GetComponentInParent<KillLog>().deleteT = 3.0f;
        //        ChatText[ChatCount].text = _msg;
        //        ++ChatCount;
        //        break;
        //}
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
        if (TestRPB || isDie)
            return;



        isDie = true;
        TestRPB = true;
    }
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(.5f);
        isJumping = true;
    }

    public void EfOn()
    {
        Balsa.Play();
    }

    private void flRP() => fl = true;

   
}
