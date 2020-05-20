using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum BulletMake
{
    None,
    Attack,
    Speed,
    Sniper
}
public class Create : MonoBehaviourPunCallbacks
{
    public BulletMake _BulletMake = BulletMake.Attack;

    private BulletManager _BulletManager;

    public PhotonView PV;
    public Transform StartTf;
    public GameObject _GunEffect;
    private int GunEffectType;
    private bool isGunTime;
    private float fGunTimer;
    public float AimY;
    private PlayerAni _Ani;

    private float fTime;

    // Sniper
    
    private Camera cam;

    private bool isBullet;

    public float testAim;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        _Ani = GetComponent<PlayerAni>();
        _BulletManager = GetComponent<BulletManager>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Move>().PV.IsMine)
            return;

        if (AimY <= 110f && AimY >= -110f)
             AimY -= Input.GetAxis("Mouse Y") * 500.0f * Time.deltaTime;
        AimY = Mathf.Clamp(AimY, -110f, 110f);


        if (GetComponent<Move>().StopT <= 0.0f)
        {
            if (_BulletManager.SniperType == 0)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    _BulletManager.AimUiChange(true, false);
                    _BulletMake = BulletMake.Attack;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    _BulletManager.AimUiChange(true, false);
                    _BulletMake = BulletMake.Speed;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    _BulletManager.AimUiChange(false, false);
                    _BulletMake = BulletMake.Sniper;
                }
            }

            // Sniper
            if (Input.GetMouseButtonDown(1))
            {
                if (_BulletMake != BulletMake.Sniper)
                    return;

                _BulletManager.SniperType++;

                if (_BulletManager.SniperType == 1) { 
                    cam.fieldOfView = 20f;
                    _BulletManager.AimUiChange(false, true);
                }
                else if (_BulletManager.SniperType == 2) cam.fieldOfView = 10f;
                else if (_BulletManager.SniperType > 2) SniperReset();
            }

            if (_BulletManager.SniperType >= 1)
            {
                SniperRay();
            }


            if (Input.GetKeyDown(KeyCode.R))
            {
                int type = (int)_BulletMake - 1;

                GetComponent<BulletManager>().BulletAdd(type);
            }

            fTime += Time.deltaTime;
            if (fTime > 0.2f) fTime = 0;


            if (GetComponent<Move>().isMove)
            {
                if (_BulletManager.SniperType > 0)
                    return;

                if (GetComponent<BulletManager>()._BulletMode == BulletManager.BulletMode.Speaker)
                {
                    if (Input.GetMouseButton(0) && fTime > 0.1f)
                    {
                        GunEffectType = 1;
                        BulletCreate();
                        fTime = 0.0f;
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        GunEffectType = 2;
                        _Ani._State = State.Attack;
                        //BulletCreate();
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    isBullet = false;
                    GunEffectType = 0;
                }
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
    private void SniperRay()
    {
        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity))
        {
            if (Input.GetMouseButton(0) && GetComponent<BulletManager>().BulletList[2].isBullet)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    hit.collider.GetComponent<BackMove>().Back1(transform.position.x, transform.position.y, transform.position.z);
                }

                GetComponent<BulletManager>().BulletUse(2);

                cam.fieldOfView = 60f;
                _BulletManager.AimUiChange(false, false);
                _BulletManager.SniperType = 0;
            }
        }
    }
    private void SniperReset()
    {
        cam.fieldOfView = 60f;
        _BulletManager.AimUiChange(true, false);
        _BulletManager.SniperType = 0;
    }
    private void FixedUpdate()
    {
        //Debug.Log(RayCastTest());
    }


    private float RayCastTest()
    {
        RaycastHit hit;
        float MaxDinstance = 15f;
        float f = 0;

        
        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDinstance))
        {
            f = hit.transform.transform.position.y;
            //Debug.Log(hit.collider.name);
        }
        return f;
    }


    public void BulletCreate()
    {
        if (GetComponent<Move>().isJumping || isBullet)
            return;


        //_Ani._State = State.Attack;
        int type = (int)_BulletMake - 1;
        if (GetComponent<BulletManager>().BulletList[type].isBullet)
        {
            if (_BulletMake == BulletMake.Attack)
                InstantiateObject("CastObj_1", StartTf.transform.position, RotVector(), type);
            else if (_BulletMake == BulletMake.Speed)
                InstantiateObject("CastObj_2", StartTf.transform.position, RotVector(), type);
            //else if (_BulletMake == BulletMake.Sniper)
            //{
            //    if(_BulletManager.SniperType == 0)
            //        InstantiateObject("CastObj_3", StartTf.transform.position, RotVector(), type);
            //}
        }

        isBullet = true;
    }

    private void InstantiateObject(string objname, Vector3 vStartPos, Vector3 vStartRot, int type)
    {
        GetComponent<BulletManager>().BulletUse(type);
        PhotonNetwork.Instantiate(objname, vStartPos, Quaternion.Euler(vStartRot));
    }

    private Vector3 RotVector(float z = 90f)
    {
        Vector3 Rot;

        Rot.x = CameraPlayer.I.transform.rotation.eulerAngles.x + AimY / testAim; //(AimY / 7);
        //Rot.x = AimY / testAim; //(AimY / 7);
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



}
