using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CastMove : MonoBehaviourPunCallbacks
{
    private Move _Move;
    public float CastSpeed = 70.0f;
    public float Dir = 0.5f;
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
        PV.RPC("AttackRPC", RpcTarget.All);
    }


    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * CastSpeed);
    }

    IEnumerator DirCheck()
    {
        yield return new WaitForSeconds(Dir);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Ground")
        {
            Destroy(gameObject);
            HitEffect(transform.position.x, transform.position.y, transform.position.z);
        }
    }

    public void HitEffect(float a, float b, float c)
    {
        PhotonNetwork.Instantiate("Hit", new Vector3(a, b, c), Quaternion.Euler(0, 0, 0));
    }



    [PunRPC]
    void AttackRPC()
    {
        Vector3 pos = transform.position;
        Vector3 PosA = transform.forward;
        RaycastHit hit;



        if (Physics.Raycast(pos, PosA, out hit, 0.5f))
        {
            if (hit.collider.CompareTag("Attack1"))
            {
                if (PV.ViewID.ToString().Substring(0, 1) == hit.collider.GetComponentInParent<Move>().PV.ViewID.ToString().Substring(0, 1))
                    return;



                GameObject[] pu = GameObject.FindGameObjectsWithTag("Player");


                for (int i = 0; i < pu.Length; i++)
                {
                    if (pu[i].GetComponentInParent<Move>().PV.ViewID.ToString().Substring(0, 1) == PV.ViewID.ToString().Substring(0, 1))
                    {
                        pu2 = pu[i];
                        pu3 = hit.collider.gameObject;
                        // Piguck();

                        if (pu3)
                            pu3.GetComponentInParent<Move>().Piguck = pu2;
                        // hit.collider.GetComponent<Move>().Piguck = pu[i];
                    }


                }
                hit.collider.GetComponent<BackMove>().Back1(transform.position.x, transform.position.y, transform.position.z);
                Destroy(this.gameObject);
                //hit.collider.GetComponent<BackMove>().PV.RPC("BackRPC", RpcTarget.All, transform.position.x, transform.position.y, transform.position.z);
            }
        }
    }
}
