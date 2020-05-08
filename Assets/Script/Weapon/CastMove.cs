using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CastMove : MonoBehaviourPunCallbacks
{
    private Move _Move; 
    public float CastSpeed = 70.0f;
    public float Dir = 0.5f;
    public GameObject[] pu;
    public PhotonView PV;
    public GameObject pu2;
    public GameObject pu3;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        StartCoroutine("DirCheck");
        _Move = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
    }
    private void Update()
    {
        Vector3 pos = transform.position;
        Vector3 PosA = transform.forward;
        RaycastHit hit;

        if(Physics.Raycast(pos,PosA,out hit,1.0f))
        {
            if(hit.collider.CompareTag("Player"))
            {
                string my = _Move.PV.ViewID.ToString();
                string you = hit.collider.gameObject.GetComponent<Move>().PV.ViewID.ToString();

                pu = GameObject.FindGameObjectsWithTag("Player");


                for(int i=0;i<pu.Length;i++)
                {
                    if (pu[i].GetComponent<Move>().PV.ViewID.ToString().Substring(0,1) == PV.ViewID.ToString().Substring(0,1))
                    {
                        pu2 = pu[i];
                        pu3 = hit.collider.gameObject;
                       // Piguck();
                      
                       // hit.collider.GetComponent<Move>().Piguck = pu[i];
                    }
                }


                PV.RPC("PiguckRPC", RpcTarget.AllBuffered);
                if (you[0] == my[0])
                    return;




                hit.collider.GetComponent<BackMove>().PV.RPC("BackRPC", RpcTarget.AllBuffered, transform.position.x, transform.position.y, transform.position.z);
                PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
        }
    }


    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * CastSpeed);
    }

    IEnumerator DirCheck()
    {
        yield return new WaitForSeconds(Dir);
        PV.RPC("DestroysRPC", RpcTarget.AllBuffered);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag == "Ground")
        {
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            PV.RPC("HitEffectRPC", RpcTarget.AllBuffered, transform.position.x, transform.position.y, transform.position.z);
        }
    }

    [PunRPC]
    void PiguckRPC()
    {
        if(pu3)
        pu3.GetComponent<Move>().Piguck = pu2;
    }

    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);

    [PunRPC]
    void DestroysRPC()
    {
        if (this.gameObject != null)
            Destroy(gameObject);
    }

    [PunRPC]
    public void HitEffectRPC(float a, float b, float c)
    {
        PhotonNetwork.Instantiate("Hit", new Vector3(a, b, c), Quaternion.Euler(0, 0, 0));
    }
}
