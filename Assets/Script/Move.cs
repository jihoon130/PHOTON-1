using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;
public class Move : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView PV;
    public string NickName;
    public string EnemyNickName;
    public float MoveSpeed = 10.0f;
    public float AngleSpeed = 0.1f;

    private Vector3 currPos;
    private Rigidbody rb;
    private Quaternion currRot;
    private Quaternion targetRotation;
    // Move
    private Transform tr;
    public float fHorizontal;
    public float fVertical;
    public float h;
    public float v;

    // jump
    public bool isGround;
    public int score;
    public Material[] _material;

    public bool isAttack;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        PV = GetComponent<PhotonView>();

        if(PV.IsMine)
        {
            NickName = PlayerPrefs.GetString("NickName");
        }
    }
    void Start()
    {
        score = 5;
        
        if(PV.IsMine)
        {
            CameraPlayer.I.target = GameObject.FindGameObjectWithTag("Player").transform;

            this.GetComponent<Renderer>().material = _material[0];
        }
        else
        {
            this.GetComponent<Renderer>().material = _material[1];
        }
    }

    private void FixedUpdate()
    {
        //controlled locally일 경우 이동(자기 자신의 캐릭터일 때)
        if (PV.IsMine)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
            tr.Translate(moveDir.normalized * MoveSpeed * Time.deltaTime, Space.Self);

            gameObject.GetComponentInChildren<TextMesh>().text = NickName;

            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                PhotonNetwork.Instantiate("333", transform.position, Quaternion.identity);
            }

            this.GetComponentInChildren<TextMesh>().text = NickName;

            if (Input.GetKeyDown(KeyCode.G))
            {
                PV.RPC("PlusScore", RpcTarget.AllBuffered);
            }
            Jump();
        }
        else
        {
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            tr.rotation = Quaternion.Lerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            gameObject.GetComponentInChildren<TextMesh>().text = EnemyNickName;
        }
    }
    private void Jump()
    {
        if(isGround)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                //CameraCol.instance.CameraJoom(CameraCol.instance.maxDistance + 4);

                rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);

                isGround = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            //CameraCol.instance.CameraJoom(CameraCol.instance.SaveDistance);
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
}
