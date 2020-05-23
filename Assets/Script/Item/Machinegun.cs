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
        GameObject.Find("MachinegunObject").GetComponent<MachinegunOBJ>().AttackChang(false);
        ResetArray();
    }
    public void MachineDeleteReset()
    {
        isMachineRay = false;
        isMachineAttack = false;
        GameObject.Find("UI_WeaponManager").GetComponent<ItemUIManager>().UIWeaponChange(true, false);
        PV.RPC("GunObjChangeRPC", RpcTarget.All, true, false);
        GetComponent<Create>()._BulletMake = BulletMake.Attack;
        GetComponent<BulletManager>()._BulletMode = BulletManager.BulletMode.Shot;
        ResetArray();
    }

    private void ResetArray()
    {
        CameraCol.instance.CameraReset();
        GetComponent<PlayerAni>()._State = State.IdleRun;
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
            if (isMachinegun || GetComponent<BulletManager>()._BulletMode == BulletManager.BulletMode.Machinegun)
                return;

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
