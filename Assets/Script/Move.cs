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

   public SkinnedMeshRenderer line;
    public MeshCollider meco1;
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
    public BoxCollider box;
    private float angle;
    private Transform cam;
    // jump
    private bool isGround;
    public int score;
    public Material[] _material;
    public GameObject oo;
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

            PV.RPC("GGRPC", RpcTarget.AllBuffered);
                Camera.main.GetComponent<SmoothFollow>().target = tr;
           // CameraFind._instance.CameraFollowObj = GameObject.FindGameObjectWithTag("Follow");
           //   cam = Camera.main.transform;
          //  this.GetComponent<Renderer>().material = _material[0];

        }
        else
        {
         //   this.GetComponent<Renderer>().material = _material[1];
        }
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            Rotate();
            PV.RPC("GetNickRPC", RpcTarget.AllBuffered);
            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<Animator>().SetBool("Attack", true);
            }

            if (Input.GetMouseButtonUp(0))
            {
                GetComponent<Animator>().SetBool("Attack",false);
            }

            if (h > 0.1f || v > 0.1f)
            {
                GetComponent<Animator>().SetBool("Run", true);
            }
            else
            {
                GetComponent<Animator>().SetBool("Run", false);
            }
        }
        }

    void FixedUpdate()
    {
        //controlled locally일 경우 이동(자기 자신의 캐릭터일 때)
        if (PV.IsMine)
        {
            gameObject.GetComponentInChildren<TextMesh>().text = NickName;
            //float MouseX = Input.GetAxis("Mouse X");
            //transform.Rotate(Vector3.up * 30.0f * MouseX);

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

            //fHorizontal = Input.GetAxisRaw("Horizontal");
            //fVertical = Input.GetAxisRaw("Vertical");

            //if (Mathf.Abs(fHorizontal) < 1 && Mathf.Abs(fVertical) < 1)
            //{

            //}
            //else
            //{
            //    tr.transform.position += tr.transform.forward * MoveSpeed * Time.deltaTime;
            //    CalculateDirection();
            //}
        }
        else
        {
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            tr.rotation = Quaternion.Lerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            gameObject.GetComponentInChildren<TextMesh>().text = EnemyNickName;
        }
    }
    private void CalculateDirection()
    {
        //// 현재 방향 각도를 구해서 카메라 각도랑 더해줌
        //angle = Mathf.Atan2(fHorizontal, fVertical);
        //angle = Mathf.Rad2Deg * angle;
        //angle += cam.eulerAngles.y;
        //Rotate();
    }
    private void Rotate()
    {
        Cursor.lockState = CursorLockMode.Locked;

        h = Input.GetAxis("Horizontal");

        v = Input.GetAxis("Vertical");




        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);


        tr.Translate(moveDir.normalized * 5.0f * Time.deltaTime, Space.Self);


        tr.Rotate(Vector3.up * 5.0f * Input.GetAxis("Mouse X"));
        //targetRotation = Quaternion.Euler(0, angle, 0);
        //tr.transform.rotation = Quaternion.Slerp(tr.transform.rotation, targetRotation, AngleSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if(isGround)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                CameraCol.instance.CameraJoom(CameraCol.instance.maxDistance + 4);

                rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);

                isGround = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            CameraCol.instance.CameraJoom(CameraCol.instance.SaveDistance);
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
        transform.localPosition = new Vector3(Random.Range(0f, 15f), 5f, Random.Range(0f, 15f));
    }

    [PunRPC]
    void PlusScore()
    {
        score++;
    }

    [PunRPC]
    public void GetNickRPC()
    {
        Mesh mesh = new Mesh();
        line.BakeMesh(mesh);
        meco1.sharedMesh = mesh;
        meco1.convex = true;
        meco1.isTrigger = true;
    }
    [PunRPC]
    public void GGRPC()
    {
        line = GetComponentInChildren<SkinnedMeshRenderer>();

    }

    public void OpenCo()
    {
        if (PV.IsMine)
            PV.RPC("OpenCoRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void OpenCoRPC()
    {
        box.enabled = !box.enabled;
    }

    public void CloseCo()
    {
        if (PV.IsMine)
            PV.RPC("CloseCoRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void CloseCoRPC()
    {
        box.enabled = !box.enabled;
    }
}
