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


    private Command _Command;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        _Command = GetComponent<Command>();
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
            float InputMouseWheel = Input.GetAxis("Mouse ScrollWheel");

            if(InputMouseWheel != 0)
            {
                _Command.Aim.AimState(1);

                GetComponent<Create>().SoundPlayer(5);
                _Command.Bulletmanager._BulletMode = BulletManager.BulletMode.Machinegun;
                GetComponent<Create>()._BulletMake = BulletMake.Machinegun;

                PV.RPC("GunObjChangeRPC", RpcTarget.All, false, true);

                GameObject.Find("UI_Item").GetComponent<ItemUIManager>().UISelectChange(false, true);
                //GameObject.Find("UI_Item").GetComponent<ItemUIManager>().ItemUIChange(false);
                //GameObject.Find("UI_Item").GetComponent<ItemUIManager>().UIWeaponChange(false, true);
                StartCoroutine("KeyTimer");

                isMachinegun = false;
            }
            else
            {
                MachineIdleChange();
            }
        }

        if(isMachineAttack)
            GetComponent<PlayerAni>()._State = State.Machinegun;


        if (isMachineAttack || isMachineRay)
        {
            if (_Command.Bulletmanager.BulletList[1].MinBullet <= 0 &&
                _Command.Bulletmanager.BulletList[1].MaxBullet <= 0)
            {
                MachineDeleteReset();
            }
            else if (_Command.Bulletmanager.BulletList[1].MinBullet <= 0 &&
                     _Command.Bulletmanager.BulletList[1].MaxBullet > 0)
            {
                MachineIdleChange();
            }
        }
    }

    public bool isItemCheck()
    {
        if (isMachinegun || isMachineAttack)
            return true;
        return false;
    }

    public void MachineIdleChange()
    {
        if (GameObject.Find("MachinegunObject") == null)
            return;

        GameObject.Find("MachinegunObject").GetComponent<MachinegunOBJ>().AttackChang(false);
        
        ResetArray();
    }
    public void MachineDeleteReset()
    {
        isMachinegun = false;
        isMachineRay = false;
        isMachineAttack = false;
        GameObject.Find("UI_Item").GetComponent<ItemUIManager>().UISelectChange(true, false);
        GameObject.Find("UI_Item").GetComponent<ItemUIManager>().UIWeaponChange(true, false);
        GameObject.Find("UI_Item").GetComponent<ItemUIManager>().ItemUIChange(false);

        PV.RPC("GunObjChangeRPC", RpcTarget.All, true, false);
        GetComponent<PlayerAni>()._State = State.IdleRun;
        GetComponent<Create>()._BulletMake = BulletMake.Attack;
        _Command.Bulletmanager._BulletMode = BulletManager.BulletMode.Shot;
        _Command.Aim.AimState(0);
        ResetArray();
    }

    public void PlayerDie()
    {
        if (!isMachinegun)
            return;

        isMachinegun = false;
        GameObject.Find("UI_Item").GetComponent<ItemUIManager>().ItemUIChange(false);
    }

    private void ResetArray()
    {
        CameraCol.instance.CameraReset();
        //GetComponent<PlayerAni>()._State = State.IdleRun;
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
        if (_Command.Bulletmanager.BulletList[1].MinBullet <= 0)
        {
            GetComponent<Create>().SoundStop(3);
            return;
        }

        EffectStart();

        GetComponent<Create>().BulletMachinegunCreate();
    }

    IEnumerator KeyTimer()
    {
        GetComponent<PlayerAni>()._State = State.Machinegun;
        yield return new WaitForSeconds(0.5f);
        isMachineAttack = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Machiengun" && PV.IsMine)
        {
            if (_Command.Bulletmanager.isGetItemCheck())
                return;

            GetComponent<Create>().SoundPlayer(4);
            isMachinegun = true;
            GameObject.Find("UI_Item").GetComponent<ItemUIManager>().ItemUIChange(true);
            _Command.Bulletmanager.BulletListAdd(1, 20, 100);
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
