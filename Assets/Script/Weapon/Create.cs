using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public enum BulletMake
{
    None,
    Attack,
    Machinegun,
    Grenade
}
public class Create : MonoBehaviourPunCallbacks
{
    public BulletMake _BulletMake = BulletMake.Attack;

    public PhotonView PV;
    public float BulletSpeed = 50f;
    public float BulletDir = 0.3f;
    public Transform StartTf;
    public Transform MachinegunStartTf;
    AudioSource Audio;
    public AudioClip[] audios;
    Move move;
    //public GameObject _GunEffect;
    private int GunEffectType;
    private bool isGunTime;
    int count = 0;
    int count1 = 0;
    private float fGunTimer;
    public float AimY;
    private PlayerAni _Ani;
    //public GameObject Effect1;
    public Image sp;
    private float fTime;
    public GameObject[] DefaultBullet;
    public GameObject[] MachinegunBulletM;
    // 재장전
    public bool isReload;
    public GameObject ReloadBulletImage; // 리로드출력 이미지
    public GameObject ReloadBG; // 재장전 백그라운드
    public Image ReloadImg; // 재장전 게이지
    RaycastHit hit;

    // Sniper

    private Camera cam;

    private bool isBullet;

    public float testAim;

    private Command _Command;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        move = GetComponent<Move>();
        Audio = GetComponentInChildren<AudioSource>();
        _Ani = GetComponent<PlayerAni>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //Effect1.GetComponent<ParticleSystem>().Stop();

        _Command = GetComponent<Command>();
    }

    private void ReloadUpdate()
    {
        if (!isReload)
            return;

        ReloadImg.fillAmount += 1f * Time.deltaTime;

        if(ReloadImg.fillAmount >= 1.0f)
        {
            _Command.Aim.AimState(1);
            int type = (int)_BulletMake - 1;
            _Command.Bulletmanager.BulletAdd(type);
            ReloadBulletImage.SetActive(false);
            ReloadBG.SetActive(false);
            isReload = false;
        }

    }
    void Update()
    {
        if (!move.PV.IsMine)
            return;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,out hit,200f, (1 << LayerMask.NameToLayer("Player")) + (1 << LayerMask.NameToLayer("Ground"))))
        {
            if(hit.collider.CompareTag("Player"))
            {
                sp.color = new Color(255, 0, 0);
            }
            else
            {
                sp.color = new Color(255, 255, 255);
            }
        }
    

        //if (move.dieOk == false)
        //{
        //    if (AimY <= 110f && AimY >= -110f)
        //        AimY -= Input.GetAxis("Mouse Y") * 500.0f * Time.deltaTime;
        //    AimY = Mathf.Clamp(AimY, -110f, 110f);
        //}
        ReloadUpdate();

        if (move.StopT <= 0.0f)
        {
            if (Input.GetKeyDown(KeyCode.R) && _Command.Machineguns.isMachineAttack && !isReload)
            {
                if (_Command.Bulletmanager.BulletList[1].MinBullet >= 20 ||
                    _Command.Bulletmanager.MaxBulletCheck(1))
                    return;

                if (_Command.Aim != null)
                    _Command.Aim.AimState(2);

                SoundPlayer(6);
                ReloadBulletImage.SetActive(true);
                ReloadBG.SetActive(true);
                ReloadImg.fillAmount = 0.0f;
                isReload = true;
            }

            fTime += Time.deltaTime;
            if (fTime > 0.2f) fTime = 0;


            if (move.isMove && !move.dieOk)
            { 
                /*
                if (Input.GetMouseButtonDown(0) && _BulletMake == BulletMake.Attack)
                {
                    if (_Command.Aim != null)
                        _Command.Aim.AimAttack(true);

                    GunEffectType = 2;
                    _Ani._State = State.Attack;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (_Command.Aim != null)
                        _Command.Aim.AimAttack(false);

                    isBullet = false;
                    GunEffectType = 0;
                }
                */


                if (_Command.Machineguns.isMachineAttack)
                {
                    if (Input.GetMouseButton(0) && !isReload)
                    {
                            _Command.Aim.AimAttack(true);

                        //if (_Ani._State == State.IdleRun)
                        {
                            CameraCol.instance.CameraJoom(1.5f);
                            GameObject.Find("MachinegunObject").GetComponent<MachinegunOBJ>().AttackChang(true);
                            _Ani._State = State.Machinegun;
                        }
                    }
                    else
                    {
                        if (_Ani._State == State.Machinegun)
                        {
                            _Command.Aim.AimAttack(false);

                            SoundStop(3);
                            isBullet = false;
                            _Command.Machineguns.MachineIdleChange();
                        }
                    }
                }
                else
                    SoundStop(3);
            }

            if (GunEffectType != 0)
            {
                PV.RPC("GunEffectTypeObj", RpcTarget.All, true);

                if (GunEffectType == 2)
                    isGunTime = true;
                else
                    GunEffectType = 3;
            }
            else
                PV.RPC("GunEffectTypeObj", RpcTarget.All, false);


            if (isGunTime)
            {
                fGunTimer += Time.deltaTime;
                if(fGunTimer > 1.0f)
                {
                    PV.RPC("GunEffectTypeObj", RpcTarget.All, false);
                    fGunTimer = 0.0f;
                    isGunTime = false;
                    GunEffectType = 0;
                }
            }

            

        }
    }

    public void EffectOn()
    {
        Debug.Log("삭제될로그_Create EffectOn");
        //Effect1.GetComponent<ParticleSystem>().Play();
    }
    public void EffectOFF()
    {
        Debug.Log("삭제될로그_Create EffectOFF");
        //Effect1.GetComponent<ParticleSystem>().Stop();
    }

    public void BulletCreate()
    {
        if (GetComponent<Move>().isJumping || isBullet)
            return;

        int a = Random.Range(0, 3);
        Audio.clip = audios[a];
        Audio.Play();

        int type = (int)_BulletMake - 1;
        if (_Command.Bulletmanager.BulletList[type].isBullet)
        {
            if (_BulletMake == BulletMake.Attack)
                InstantiateObject("CastObj_1", StartTf.transform.position, RotVector(), type);
        }

        isBullet = true;
    }

    public void BulletMachinegunCreate()
    {
        int type = (int)_BulletMake - 1;
        if (_Command.Bulletmanager.BulletList[type].isBullet)
        {
            SoundPlayer(3);

            InstantiateObject2("Machinegun_bullet", MachinegunStartTf.transform.position, RotVector(), type);
        }
    }

    private void InstantiateObject(string objname, Vector3 vStartPos, Vector3 vStartRot, int type)
    {
        if (!hit.collider)
            return;

        _Command.Bulletmanager.BulletUse(type);
        for (int i = count; i < DefaultBullet.Length; i++)
        {
            if(!DefaultBullet[i].activeSelf)
            {
                DefaultBullet[i].GetComponent<BasicBullet>().Point = hit.point;
                DefaultBullet[i].transform.position = vStartPos;
                PV.RPC("DefaultBulletOnRPC", RpcTarget.All, i);
                count++;
                if (count == DefaultBullet.Length)
                    count = 0;
                break;
            }
        }
    }
        

    private void InstantiateObject2(string objname, Vector3 vStartPos, Vector3 vStartRot, int type)
    {
        if (!hit.collider)
            return;

        _Command.Bulletmanager.BulletUse(type);
        for (int i = count1; i < MachinegunBulletM.Length; i++)
        {
            if (!MachinegunBulletM[i].activeSelf)
            {
                MachinegunBulletM[i].GetComponent<MachinegunBullet>().Point = hit.point;
                MachinegunBulletM[i].transform.position = vStartPos;
                PV.RPC("MachinegunBulletOnRPC", RpcTarget.All, i);
                count1++;
                if (count1 == MachinegunBulletM.Length)
                    count1 = 0;
                break;
            }
        }
    }

    [PunRPC]
    void DefaultBulletOnRPC(int i)
    {
        DefaultBullet[i].SetActive(true);

    }

    [PunRPC]
    void MachinegunBulletOnRPC(int i)
    {
        MachinegunBulletM[i].SetActive(true);
    }

    private Vector3 RotVector(float z = 90f)
    {
        Vector3 Rot;

        Rot.x = CameraPlayer.I.transform.rotation.eulerAngles.x + AimY / testAim; 
        Rot.y = transform.rotation.eulerAngles.y;
        Rot.z = z;
        return Rot;
    }

    public void BulletIsCheckd(bool isCheckd) => isBullet = isCheckd;



    [PunRPC]
    public void GunEffectTypeObj(bool type)
    {
        //_GunEffect.SetActive(type);
    }


    public void SoundPlayer(int type)
    {
        Audio.clip = audios[type];
        Audio.Play();
    }
    public void SoundStop(int type)
    {
        if (Audio.clip == audios[type])
            Audio.Stop();
    }

    public void ItemSoundEvent() => SoundPlayer(8);
}
