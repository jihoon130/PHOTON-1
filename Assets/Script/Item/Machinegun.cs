using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Machinegun : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    public GameObject GunObj;
    public GameObject MachinegunObj;


    [HideInInspector]
    public bool isMachinegun; // 머신건 UI 사용 유무
    public bool isMachineAttack;





    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        GunObjChange(true, false);
    }

    void Update()
    {
        if (!PV.IsMine)
            return;

        if(isMachinegun)
        {
            if(Input.GetMouseButtonDown(1))
            {
                GunObjChange(false, true);
                GameObject.Find("UI_ItemManager").GetComponent<ItemUIManager>().ItemUIChange(false);
                isMachineAttack = true;
                isMachinegun = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Machiengun" && PV.IsMine)
        {
            isMachinegun = true;
            GameObject.Find("UI_ItemManager").GetComponent<ItemUIManager>().ItemUIChange(true);
            Destroy(collision.gameObject);
        }
    }

    public void GunObjChange(bool gun1, bool gun2)
    {
        GunObj.SetActive(gun1);
        MachinegunObj.SetActive(gun2);
    }
}
