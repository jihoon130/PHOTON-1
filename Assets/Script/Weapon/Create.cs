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

    private BulletManager _BulletManager;

    public PhotonView PV;
    public float BulletSpeed = 50f;
    public float BulletDir = 0.3f;
    public Transform StartTf;
    public Transform MachinegunStartTf;
    AudioSource Audio;
    public AudioClip[] audios;

    public GameObject _GunEffect;
    private int GunEffectType;
    private bool isGunTime;
    private float fGunTimer;
    public float AimY;
    private PlayerAni _Ani;
    public GameObject Effect1;
    private float fTime;


    // 재장전
    public bool isReload;
    public GameObject ReloadBulletImage; // 리로드출력 이미지
    public GameObject ReloadBG; // 재장전 백그라운드
    public Image ReloadImg; // 재장전 게이지

    // Sniper

    private Camera cam;

    private bool isBullet;

    public float testAim;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        Audio = GetComponentInChildren<AudioSource>();
        _Ani = GetComponent<PlayerAni>();
        _BulletManager = GetComponent<BulletManager>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Effect1.GetComponent<ParticleSystem>().Stop();
    }

    private void ReloadUpdate()
    {
        if (!isReload)
            return;

        ReloadImg.fillAmount += 1f * Time.deltaTime;

        if(ReloadImg.fillAmount >= 1.0f)
        {
            int type = (int)_BulletMake - 1;
            GetComponent<BulletManager>().BulletAdd(type);
            ReloadBulletImage.SetActive(false);
            ReloadBG.SetActive(false);
            isReload = false;
        }

    }
    void Update()
    {
        if (!GetComponent<Move>().PV.IsMine)
            return;

        if (AimY <= 110f && AimY >= -110f)
             AimY -= Input.GetAxis("Mouse Y") * 500.0f * Time.deltaTime;
        AimY = Mathf.Clamp(AimY, -110f, 110f);

        ReloadUpdate();

        if (GetComponent<Move>().StopT <= 0.0f)
        {
            if (Input.GetKeyDown(KeyCode.R) && GetComponent<Machinegun>().isMachineAttack && !isReload)
            {
                if (GetComponent<BulletManager>().BulletList[1].MinBullet >= 20 ||
                    GetComponent<BulletManager>().MaxBulletCheck(1))
                    return;


                SoundPlayer(6);
                ReloadBulletImage.SetActive(true);
                ReloadBG.SetActive(true);
                ReloadImg.fillAmount = 0.0f;
                isReload = true;
            }

            fTime += Time.deltaTime;
            if (fTime > 0.2f) fTime = 0;


            if (GetComponent<Move>().isMove)
            {
                if (Input.GetMouseButtonDown(0) && _BulletMake == BulletMake.Attack)
                {
                    GunEffectType = 2;
                    _Ani._State = State.Attack;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    isBullet = false;
                    GunEffectType = 0;
                }


                if (GetComponent<Machinegun>().isMachineAttack )
                {
                    if (Input.GetMouseButton(0) && !isReload)
                    {
                        //if (_Ani._State == State.IdleRun)
                        {
                            CameraCol.instance.CameraJoom(2.5f);
                            GameObject.Find("MachinegunObject").GetComponent<MachinegunOBJ>().AttackChang(true);
                            _Ani._State = State.Machinegun;
                        }
                    }
                    else
                    {
                        if (_Ani._State == State.Machinegun)
                        {
                            SoundStop(3);
                            isBullet = false;
                            GetComponent<Machinegun>().MachineIdleChange();
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
        Effect1.GetComponent<ParticleSystem>().Play();
    }
    public void EffectOFF()
    {
        Effect1.GetComponent<ParticleSystem>().Stop();
    }

    public void BulletCreate()
    {
        if (GetComponent<Move>().isJumping || isBullet)
            return;

        int a = Random.Range(0, 3);
        Audio.clip = audios[a];
        Audio.Play();

        int type = (int)_BulletMake - 1;
        if (GetComponent<BulletManager>().BulletList[type].isBullet)
        {
            if (_BulletMake == BulletMake.Attack)
                InstantiateObject("CastObj_1", StartTf.transform.position, RotVector(), type);
        }

        isBullet = true;
    }

    public void BulletMachinegunCreate()
    {
        int type = (int)_BulletMake - 1;
        if (GetComponent<BulletManager>().BulletList[type].isBullet)
        {
            SoundPlayer(3);

            InstantiateObject("Machinegun_bullet", MachinegunStartTf.transform.position, RotVector(), type);
        }
    }

    private void InstantiateObject(string objname, Vector3 vStartPos, Vector3 vStartRot, int type)
    {
        GetComponent<BulletManager>().BulletUse(type);
      GameObject _bullet = PhotonNetwork.Instantiate(objname, vStartPos, Quaternion.Euler(vStartRot));
        CastMove _bulletScript = _bullet.GetComponent<CastMove>();
        _bulletScript.CastSpeed = BulletSpeed;
        _bulletScript.Dir = BulletDir;
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
        _GunEffect.SetActive(type);
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

}
