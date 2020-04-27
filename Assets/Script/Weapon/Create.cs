using System.Collections;
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

            if (GetComponent<BulletManager>()._BulletMode == BulletManager.BulletMode.Speaker)
            {
                if (Input.GetMouseButton(0) && fTime > 0.1f)
                {
                    BulletCreate();
                    fTime = 0.0f;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    BulletCreate();
                    _Ani._State = State.Attack;
                    
                }
                else if(Input.GetMouseButtonUp(0))
                {
                    _Ani._State = State.IdleRun;
                }
            }
        }
    }

    public void BulletCreate()
    {
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
        Rot.x = transform.rotation.eulerAngles.x;
        Rot.y = transform.rotation.eulerAngles.y;
        Rot.z = z;

        return Rot;
    }
}
