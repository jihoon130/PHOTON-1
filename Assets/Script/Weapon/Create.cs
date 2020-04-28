﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum BulletMake
{
    None,
    Attack,
    Speed
}
public class Create : MonoBehaviourPunCallbacks
{
    
    public BulletMake _BulletMake = BulletMake.Attack;

    public PhotonView PV;
    public Transform StartTf;
    public GameObject _GunEffect;
    private int GunEffectType;
    private bool isGunTime;
    private float fGunTimer;
    public float Y;
    private PlayerAni _Ani;

    private float fTime;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        _Ani = GetComponent<PlayerAni>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!GetComponent<Move>().PV.IsMine)
            return;

        if (Y <= 285f && Y >= -442f)
            Y -= Input.GetAxis("Mouse Y") * 300.0f * Time.deltaTime;
        else if (Y >= 285f)
            Y = 285f;
        else if (Y <= -442f)
            Y = -442f;

        if (GetComponent<Move>().StopT <= 0.0f)
        {
            //if(Input.GetKeyDown(KeyCode.G))
            //{
            //    Debug.Log(BulletManager.I._BulletMode);
            //}

            if (Input.GetKeyDown(KeyCode.Alpha1))
                _BulletMake = BulletMake.Attack;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                _BulletMake = BulletMake.Speed;

            if (Input.GetKeyDown(KeyCode.R))
            {
                int type = (int)_BulletMake - 1;

                GetComponent<BulletManager>().BulletAdd(type);
            }

            fTime += Time.deltaTime;
            if (fTime > 0.2f) fTime = 0;


            if (GetComponent<Move>().isMove)
            {
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
                        BulletCreate();
                    }
                }

                if (Input.GetMouseButtonUp(0))
                    GunEffectType = 0;
            }



            if (GunEffectType != 0)
            {
                PV.RPC("GunEffectTypeObj", RpcTarget.AllBuffered, true);

                if (GunEffectType == 2)
                    isGunTime = true;
                else
                    GunEffectType = 3;
            }
            else
                PV.RPC("GunEffectTypeObj", RpcTarget.AllBuffered, false);


            if (isGunTime)
            {
                fGunTimer += Time.deltaTime;
                if(fGunTimer > 1.0f)
                {
                    PV.RPC("GunEffectTypeObj", RpcTarget.AllBuffered, false);
                    fGunTimer = 0.0f;
                    isGunTime = false;
                    GunEffectType = 0;
                }
            }
        }
    }

    public void BulletCreate()
    {
        if (GetComponent<Move>().isJumping)
            return;

        _Ani._State = State.Attack;
        int type = (int)_BulletMake - 1;
        if (GetComponent<BulletManager>().BulletList[type].isBullet)
        {
            if (_BulletMake == BulletMake.Attack)
                InstantiateObject("CastObj_1", StartTf.transform.position, RotVector(), type);
            else if (_BulletMake == BulletMake.Speed)
                InstantiateObject("CastObj_2", StartTf.transform.position, RotVector(), type);
        }
        
    }

    private void InstantiateObject(string objname, Vector3 vStartPos, Vector3 vStartRot, int type)
    {
        GetComponent<BulletManager>().BulletUse(type);
        PhotonNetwork.Instantiate(objname, vStartPos, Quaternion.Euler(vStartRot));
    }

    private Vector3 RotVector(float z = 90f)
    {
        Vector3 Rot;
        Rot.x = transform.rotation.x + Y/10;
        Rot.y = transform.rotation.eulerAngles.y;
        Rot.z = z;

        return Rot;
    }

   
    [PunRPC]
    public void GunEffectTypeObj(bool type)
    {
        _GunEffect.SetActive(type);
    }



}
