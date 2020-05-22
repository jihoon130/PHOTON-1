using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Machinegun : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    public GameObject GunObj;
    public GameObject MachinegunObj;


    public bool isMachinegun; // 머신건 UI 사용 유무
    public bool isMachineAttack;
    public bool isMachineRay;

    public Transform MachinegunStartPoint;

    private float timer;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        GunObjChangeRPC(true, false);

    }

    void Update()
    {
        if (isMachineRay)
        {
            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                RayCastBullet();
                timer = 0.0f;
            }
        }

        if (!PV.IsMine)
            return;

        Debug.Log("isMachineRay : " + isMachineRay);

        if(isMachinegun)
        {
            if(Input.GetMouseButtonDown(1))
            {
                PV.RPC("GunObjChangeRPC", RpcTarget.All, false, true);
                GameObject.Find("UI_ItemManager").GetComponent<ItemUIManager>().ItemUIChange(false);
                StartCoroutine("KeyTimer");
                isMachinegun = false;
            }
        }

        if(isMachineAttack)
        {
            if (GetComponent<BulletManager>().BulletList[1].MinBullet <= 0 &&
                GetComponent<BulletManager>().BulletList[1].MaxBullet <= 0)
            {
                isMachineRay = false;
                GunObjChangeRPC(true, false);
                GetComponent<Create>()._BulletMake = BulletMake.Attack;
                GetComponent<PlayerAni>()._State = State.IdleRun;
                CameraCol.instance.CameraReset();
                isMachineAttack = false;
            }
        }
    }

    private void RayCastBullet()
    {
        if (GetComponent<BulletManager>().BulletList[1].MinBullet <= 0)
            return;

        GetComponent<BulletManager>().BulletUse(1);

        RaycastHit hit;

        if (Physics.Raycast(MachinegunStartPoint.transform.position, MachinegunStartPoint.transform.forward, out hit, Mathf.Infinity))
        {
            if(hit.collider.tag == "Attack1")
            {
                PhotonNetwork.Instantiate("Hit", new Vector3(hit.transform.position.x, hit.transform.position.y + 0.5f, hit.transform.position.z), Quaternion.Euler(0, 0, 0));
            }
            else
                PhotonNetwork.Instantiate("Hit", hit.transform.position, Quaternion.Euler(0, 0, 0));

            if (hit.collider.CompareTag("Attack1"))
            {
                hit.collider.GetComponent<BackMove>().Back1(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
            }
        }
    }

    IEnumerator KeyTimer()
    {
        yield return new WaitForSeconds(0.5f);
        isMachineAttack = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Machiengun" && PV.IsMine)
        {
            isMachinegun = true;
            GameObject.Find("UI_ItemManager").GetComponent<ItemUIManager>().ItemUIChange(true);
            collision.gameObject.GetComponent<ItemDestroy>().PV.RPC("DestroyRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    public void GunObjChangeRPC(bool gun1, bool gun2)
    {
        GunObj.SetActive(gun1);
        MachinegunObj.SetActive(gun2);
    }
}
