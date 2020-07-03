using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CastMove : MonoBehaviourPunCallbacks
{
    public enum BulletMode { Attack, Machinegun, Grenade}
    public BulletMode _BulletMode = BulletMode.Attack;

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
    private void Update() => Attack();
    private void FixedUpdate()
    {
        if(_BulletMode == BulletMode.Grenade)
            transform.Translate(Vector3.left * Time.deltaTime * CastSpeed);
        else
            transform.Translate(Vector3.forward * Time.deltaTime * CastSpeed);
    }

    IEnumerator DirCheck()
    {
        if (_BulletMode == BulletMode.Grenade)
            StopCoroutine("DirCheck");

        yield return new WaitForSeconds(Dir);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Ground" || other.collider.tag == "Wall" || other.collider.tag == "Fance")
        {
            if (_BulletMode == BulletMode.Grenade)
                return;

            Destroy(gameObject);
            HitEffect(transform.position.x, transform.position.y, transform.position.z);

            if (other.collider.tag == "Fance")
                other.collider.GetComponent<FenceObj>().DestroyRPC();
        }
    }

    public void HitEffect(float a, float b, float c)
    {
        if (_BulletMode == BulletMode.Grenade)
            return;

        string effectName = null;

        if (_BulletMode == BulletMode.Attack) effectName = "Hit";
        else if (_BulletMode == BulletMode.Machinegun) effectName = "water_hit";

        PhotonNetwork.Instantiate(effectName, new Vector3(a, b, c), Quaternion.identity);
    }



  //  [PunRPC]
    void Attack()
    {

        if (!this.gameObject)
            return;



        //Vector3 pos = transform.position;
        //Vector3 PosA = transform.forward;
        //RaycastHit hit;



        //if (Physics.Raycast(pos, PosA, out hit, 0.5f))
        //{
        //    if (hit.collider.CompareTag("Attack1"))
        //    {
        //        if (PV.ViewID.ToString().Substring(0, 1) == hit.collider.GetComponentInParent<Move>().PV.ViewID.ToString().Substring(0, 1))
        //            return;


        //        GameObject[] pu = GameObject.FindGameObjectsWithTag("Player");

        //        for (int i = 0; i < pu.Length; i++)
        //        {
        //            if (pu[i].GetComponentInParent<Move>().PV.ViewID.ToString().Substring(0, 1) == PV.ViewID.ToString().Substring(0, 1))
        //            {
        //                pu2 = pu[i];
        //                pu3 = hit.collider.gameObject;
        //                // Piguck();

        //                if (pu3)
        //                    pu3.GetComponentInParent<Move>().Piguck = pu2;
        //                // hit.collider.GetComponent<Move>().Piguck = pu[i];
        //            }
        //        }
        //        hit.collider.GetComponent<BackMove>().Back1(transform.position.x, transform.position.y, transform.position.z);
        //        Destroy(gameObject);
        //        //hit.collider.GetComponent<BackMove>().PV.RPC("BackRPC", RpcTarget.All, transform.position.x, transform.position.y, transform.position.z);
        //    }
        //}
    }
}
