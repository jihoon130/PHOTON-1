using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Machinegun : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    public GameObject AttackEffect;
    public GameObject AttackTrashEffect;

    public enum Mode { IdleRun, Attack};
    public Mode _Mode = Mode.IdleRun;


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
            GetComponent<Move>().fHorizontal = 0;
            GetComponent<Move>().fVertical = 0;

            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                RayCastBullet();
                timer = 0.0f;
            }
        }
        else EffectStop();


        if (!PV.IsMine)
            return;

        if(isMachinegun)
        {
            if(Input.GetMouseButtonDown(1))
            {
                //GetComponent<PlayerAni>()._State = State.Machinegun;

                GetComponent<BulletManager>()._BulletMode = BulletManager.BulletMode.Machinegun;
                GetComponent<Create>()._BulletMake = BulletMake.Machinegun;

                PV.RPC("GunObjChangeRPC", RpcTarget.All, false, true);

                GameObject.Find("UI_WeaponManager").GetComponent<ItemUIManager>().ItemUIChange(false);
                GameObject.Find("UI_WeaponManager").GetComponent<ItemUIManager>().UIWeaponChange(false, true);
                StartCoroutine("KeyTimer");
                isMachinegun = false;
            }
        }

        if(isMachineAttack || isMachineRay)
        {
            if (GetComponent<BulletManager>().BulletList[1].MinBullet <= 0 &&
                GetComponent<BulletManager>().BulletList[1].MaxBullet <= 0)
            {
                MachineDeleteReset();
            }
            else if (GetComponent<BulletManager>().BulletList[1].MinBullet <= 0 &&
                     GetComponent<BulletManager>().BulletList[1].MaxBullet > 0)
            {
                MachineIdleChange();
            }
        }
    }

    public void MachineIdleChange()
    {
        CameraCol.instance.CameraReset();
        GameObject.Find("MachinegunObject").GetComponent<MachinegunOBJ>().AttackChang(false);
        //_Mode = Mode.IdleRun;
        GameObject.Find("UI_WeaponManager").GetComponent<ItemUIManager>().UIWeaponChange(true, false);
        GetComponent<PlayerAni>()._State = State.IdleRun;
    }
    public void MachineDeleteReset()
    {
        isMachineRay = false;
        GameObject.Find("UI_WeaponManager").GetComponent<ItemUIManager>().UIWeaponChange(true, false);
        PV.RPC("GunObjChangeRPC", RpcTarget.All, true, false);
        GetComponent<Create>()._BulletMake = BulletMake.Attack;
        GetComponent<PlayerAni>()._State = State.IdleRun;
        CameraCol.instance.CameraReset();
        isMachineAttack = false;
    }
    public void EffectStart()
    {
        AttackEffect.GetComponent<ParticleSystem>().Play();
        AttackTrashEffect.GetComponent<ParticleSystem>().Play();
    }
    public void EffectStop()
    {
        AttackEffect.GetComponent<ParticleSystem>().Stop();
        AttackTrashEffect.GetComponent<ParticleSystem>().Stop();
    }
    private void RayCastBullet()
    {
        if (GetComponent<BulletManager>().BulletList[1].MinBullet <= 0)
            return;

        EffectStart();

        GetComponent<Create>().BulletMachinegunCreate();

        //GetComponent<BulletManager>().BulletUse(1);

        //RaycastHit hit;

        //if (Physics.Raycast(MachinegunStartPoint.transform.position, MachinegunStartPoint.transform.forward, out hit, Mathf.Infinity))
        //{
        //    Transform points = hit.transform;
        //
        //    PhotonNetwork.Instantiate("MachinegunHit", new Vector3(points.position.x, points.position.y + 0.5f, points.position.z), Quaternion.Euler(0, 0, 0));
        //
        //    if (hit.collider.CompareTag("Attack1"))
        //    {
        //        hit.collider.GetComponent<BackMove>().PV.RPC("BackRPC", RpcTarget.All, hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
        //        //.Back1(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
        //
        //        if (PV.ViewID.ToString().Substring(0, 1) == hit.collider.GetComponentInParent<Move>().PV.ViewID.ToString().Substring(0, 1))
        //            return;
        //
        //
        //    }
        //}
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
            GameObject.Find("UI_WeaponManager").GetComponent<ItemUIManager>().ItemUIChange(true);
            GetComponent<BulletManager>().BulletListAdd(1);
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
