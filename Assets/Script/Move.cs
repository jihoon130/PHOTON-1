using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;
public class Move : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView PV;
    private PlayerAni _PlayerAni;

    public bool isMove;


    public string NickName;
    public string EnemyNickName;
    public float MoveSpeed = 10.0f;
    public float AngleSpeed = 0.1f;
    public float daepoT = 0.0f;
    // Water
    public GameObject Boonsoo;

    private Vector3 currPos;
    private Rigidbody rb;
    private Quaternion currRot;
    private Quaternion targetRotation;
    // Move
    private Transform tr;
    public float fHorizontal;
    public float fVertical;

    public float StopT=0.0f;
    // jump
    public bool isGround;
    public bool isJumping;
    public bool isJumpDown;
    private float fJumptime;
    public int score;
    //public Material[] _material;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        PV = GetComponent<PhotonView>();
        _PlayerAni = GetComponent<PlayerAni>();

        if (PV.IsMine)
        {
            NickName = PlayerPrefs.GetString("NickName");
        }
    }
    void Start()
    {
        score = 5;
        isMove = true;
        if (PV.IsMine)
        {
            CameraPlayer.I.target = GameObject.FindGameObjectWithTag("Player").transform;

            //this.GetComponent<Renderer>().material = _material[0];
        }
        else
        {
            //this.GetComponent<Renderer>().material = _material[1];
        }
    }


   
    private void FixedUpdate()
    {
        //controlled locally일 경우 이동(자기 자신의 캐릭터일 때)
        if (PV.IsMine)
        {
            if (StopT <= 0.0f)
            {
                if (!isMove)
                    return;

              


                fHorizontal = Input.GetAxisRaw("Horizontal");
                fVertical = Input.GetAxisRaw("Vertical");

                if (fVertical < 0) MoveSpeed = 5f;
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

    private void Update()
    {
        if (PV.IsMine)
        {
            if(daepoT<=20.0f)
            {
                daepoT += Time.deltaTime;
            }


            if (StopT <= 0.0f)
            {
                if (!isMove)
                    return;


                if (Input.GetKeyDown(KeyCode.Space))
                {
                    fJumptime = 0;
                  //  rb.velocity = Vector3.zero;
                    StartCoroutine("Fade");
                    _PlayerAni._State = State.Jump_Start;
                }


                if (isJumping)
                {
                    RaycastHit hit;
                    Vector3 pos = transform.position;
                    Vector3 dir = -transform.up;
                    if (Physics.Raycast(pos, dir, out hit, 1f))
                    {
                        Debug.Log(hit.collider.tag);
                        if (hit.collider.CompareTag("Ground"))
                        {
                            PV.RPC("DownRPC", RpcTarget.AllBuffered);
                        }
                    }
                }

                if (daepoT>=20.0f && Input.GetKeyDown(KeyCode.V))
                {
                    daepoT = 0.0f;
                    PV.RPC("OpenWaterRPC", RpcTarget.AllBuffered);
                }

                if (Input.GetKeyDown(KeyCode.G))
                {
                    PV.RPC("PlusScore", RpcTarget.AllBuffered);
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
        if (!isGround)
            return;

        rb.AddForce(Vector3.up * 7f, ForceMode.Impulse);

        isGround = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        //if(collision.gameObject.tag == "Ground")
        //{
        //    if(isJumpDown && fJumptime > 1.3f)
        //    {
        //        fHorizontal = 0;
        //        fVertical = 0;

        //        _PlayerAni._State = State.Jump_End;
        //        isJumping = false;
        //        isJumpDown = false;
        //        fJumptime = 0.0f;
        //    }

        //    isGround = true;
        //}
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //통신을 보내는 
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
            stream.SendNext(NickName.ToString());
        }
        //클론이 통신을 받는 
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            EnemyNickName = (string)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void ResetPos()
    {
        rb.velocity = Vector3.zero;
        transform.localPosition = new Vector3(Random.Range(27, -27), 5f, Random.Range(4, 5));
    }

    [PunRPC]
    void PlusScore()
    {
        score++;
    }

    [PunRPC]
    public void SpeedSetting()
    {
        if(PV.IsMine)
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

    [PunRPC]
    public void MoveTrue()
    {
        isMove = true;
    }

    [PunRPC]
    public void MoveFalse()
    {
        isMove = false;
    }

    [PunRPC]
    public void DownRPC()
    {
        fHorizontal = 0;
        fVertical = 0;

        _PlayerAni._State = State.Jump_End;
        isJumping = false;
        isJumpDown = false;

     isGround = true;
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(.5f);
        isJumping = true;
    }
}
