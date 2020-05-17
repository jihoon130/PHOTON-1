using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;
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
    public float MoveSpeed = 10.0f;
    public float AngleSpeed = 0.1f;
    public float daepoT = 0.0f;
    // Water
    public GameObject Boonsoo;
    public ScoreManager scoreM;
    private Vector3 currPos;
    private Rigidbody rb;
    private Quaternion currRot;
    private Quaternion targetRotation;
    // Move
    private Transform tr;
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

                if (fVertical < 0) MoveSpeed = 6f;
                else MoveSpeed = 10f;

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
    public void BlinkForward()
    {
        RaycastHit hit;
        Vector3 destination = transform.position + transform.forward * 5f;

        if (Physics.Linecast(transform.position, destination, out hit))
        {
            destination = transform.position + transform.forward * (hit.distance - 1f);
        }

        if (Physics.Raycast(destination, -Vector3.up, out hit))
        {
            destination = hit.point;
            destination.y = 0.5f;
            transform.position = destination;
        }
    }
    private void Update()
    {
        if (PV.IsMine)
        {
            if (Piguck)
                StartCoroutine("DestroyPiguck");


            if (isDie)
            {
                rb.velocity = Vector3.zero;

            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (fVertical <= 0)
                    return;

                BlinkForward();
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


                    if (Physics.Raycast(pos, dir, out hit, 2.0f))
                    {
                        if (hit.collider.CompareTag("Ground"))
                        {
                            DownR();
                        }
                    }
                }
                else
                {
                    if (_PlayerAni._State == State.Jump_Ing)
                        _PlayerAni._State = State.Jump_End;
                }

                if (daepoT >= 20.0f && Input.GetKeyDown(KeyCode.V))
                {
                    daepoT = 0.0f;
                    PV.RPC("OpenWaterRPC", RpcTarget.All);
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
        rb.AddForce(Vector3.up * 4f, ForceMode.Impulse);
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
            Piguck = null;
            // PV.RPC("PlusScoreRPC", RpcTarget.All);
        }

        rb.velocity = Vector3.zero;

        transform.localPosition = new Vector3(0, 10, Random.Range(-3, 3));

        isDie = false;
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

    [PunRPC]
    public void OpenWaterRPC()
    {
        isMove = false;
        Boonsoo.SetActive(true);
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

    [PunRPC]
    public void DestroyRPC() => Destroy(gameObject);
}
