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
        GunObjChangeRPC(true, false);
    }

    void Update()
    {
        if (!PV.IsMine)
            return;

        if(isMachinegun)
        {
            if(Input.GetMouseButtonDown(1))
            {
                PV.RPC("GunObjChangeRPC", RpcTarget.All, false, true);
                GameObject.Find("UI_ItemManager").GetComponent<ItemUIManager>().ItemUIChange(false);
                StartCoroutine("KeyTimer");
                isMachinegun = false;
            }
        }
    }

    IEnumerator KeyTimer()
    {
        yield return new WaitForSeconds(1f);
        isMachineAttack = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Machiengun" && PV.IsMine)
        {
            isMachinegun = true;
            GameObject.Find("UI_ItemManager").GetComponent<ItemUIManager>().ItemUIChange(true);
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
