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

    private float fTime;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _BulletMake = BulletMake.Attack;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            _BulletMake = BulletMake.Speed;

        if (Input.GetKeyDown(KeyCode.R))
        {
            int type = (int)_BulletMake - 1;

            BulletManager.I.BulletAdd(type);
        }

        if (!PV.IsMine)
            return;

        fTime += Time.deltaTime;
        if (fTime > 0.2f) fTime = 0;

        if (BulletManager.I._BulletMode == BulletManager.BulletMode.Speaker)
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
            }
        }
    }

    private void BulletCreate()
    {
        int type = (int)_BulletMake - 1;
        
        if (BulletManager.I.BulletList[type].isBullet)
        {
            if (_BulletMake == BulletMake.Attack)
                InstantiateObject("CastObj_1", StartTf.transform.position, RotVector(), type);
            else if (_BulletMake == BulletMake.Speed)
                InstantiateObject("CastObj_2", StartTf.transform.position, RotVector(), type);
        }
    }

    private void InstantiateObject(string objname, Vector3 vStartPos, Vector3 vStartRot, int type)
    {
        BulletManager.I.BulletUse(type);
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
