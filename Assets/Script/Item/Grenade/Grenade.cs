using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Grenade : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public GameObject GunObj;
    public GameObject GrenadeObj;
    public Transform CreatePos;
    public bool isGreande;
    private bool isDelete;
    private bool isGreandeAttack;
    private bool isBullet;
    public Transform trs;
    public GameObject[] Boom;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
            return;

        if (Input.GetMouseButton(1) && isGreande)
        {
            GetComponent<Create>().SoundPlayer(5);
            GetComponent<BulletManager>()._BulletMode = BulletManager.BulletMode.Grenade;
            GetComponent<Create>()._BulletMake = BulletMake.Grenade;
            PV.RPC("ObjChangeRPC", RpcTarget.All, false, true);
            GameObject.Find("UI_Item").GetComponent<ItemUIManager>().UISelectChange(false, true);
            isGreandeAttack = true;
            isGreande = false;
        }

        if (GetComponent<BulletManager>().BulletList[2].MinBullet <= 0)
        {
            if (!isDelete)
                return;

            DeleteGreade();
        }

        if (isGreandeAttack)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GetComponent<BulletManager>().BulletList[2].MinBullet < 0)
                    return;
                
                GetComponent<PlayerAni>()._State = State.Greande;

                isBullet = false;
            }
        }
    }

    public bool isItemCheck()
    {
        if (isGreande || isGreandeAttack)
            return true;
        return false;
    }

    public void DeleteGreade()
    {
        isDelete = true;

        isGreande = false;

        PV.RPC("ObjChangeRPC", RpcTarget.All, true, false);
        GetComponent<BulletManager>()._BulletMode = BulletManager.BulletMode.Shot;
        GetComponent<Create>()._BulletMake = BulletMake.Attack;

        GameObject.Find("UI_Item").GetComponent<ItemUIManager>().UISelectChange(true, false);
        GameObject.Find("UI_Item").GetComponent<ItemUIManager>().ItemUIChange(false, 1);
        isGreandeAttack = false;

        isDelete = false;
    }

    public void CraeteCrenade()
    {
        if (isBullet)
            return;

        if (GetComponent<BulletManager>().BulletList[2].isBullet)
        {
            for(int i=0;i<Boom.Length;i++)
            {
                if(Boom[i].activeInHierarchy==false)
                {
                    PV.RPC("InitBoomRPC", RpcTarget.All, i);
                    break;
                }
            }

            GetComponent<BulletManager>().BulletUse(2);
        }





        isBullet = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Grenade" && PV.IsMine)
        {
            if (GetComponent<BulletManager>().isGetItemCheck())
                return;

            isDelete = true;
            GetComponent<Create>().SoundPlayer(4);
            GameObject.Find("UI_Item").GetComponent<ItemUIManager>().ItemUIChange(true, 1);
            GetComponent<BulletManager>().BulletListAdd(2, 5, 0);
            isGreande = true;
            collision.gameObject.GetComponent<ItemDestroy>().PV.RPC("DestroyRPC", RpcTarget.All);
        }
    }


    [PunRPC]
    public void ObjChangeRPC(bool gun1, bool gun2)
    {
        GunObj.SetActive(gun1);
        GrenadeObj.SetActive(gun2);
    }


    [PunRPC]
    void InitBoomRPC(int i)
    {
        Boom[i].SetActive(true);
    }
}
