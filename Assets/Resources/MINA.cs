using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
public class MINA : MonoBehaviour
{
  public  PhotonView pv;
    public bool OK=false;
    private Image ChargeGage;
    public PlayerAni _PlayerAni;
    public GameObject[] Effect;
    bool FullCharge=false;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        _PlayerAni = GetComponentInParent<PlayerAni>();
        ChargeGage = GameObject.Find("ChargeGage").GetComponent<Image>();
        Effect[2] = GameObject.Find("hit");
        Effect[3] = GameObject.Find("charge_hit");
    }


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (ChargeGage.fillAmount <= 1f) ChargeGage.fillAmount += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _PlayerAni._State = State.Attack;
            ChargeGage.fillAmount = 0f;
            FullCharge = false;
          if(pv.IsMine) pv.RPC("EffectAllOffRPC", RpcTarget.All);
        }

        if(pv.IsMine &&!Effect[0].activeInHierarchy &&ChargeGage.fillAmount >= 0.1f && ChargeGage.fillAmount <= 0.9f)
        {
            pv.RPC("EffectOnRPC", RpcTarget.All);
        }
        else if(pv.IsMine && !Effect[1].activeInHierarchy && ChargeGage.fillAmount >= 1f)
        {
            FullCharge = true;
            pv.RPC("EffectOffRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    void EffectOnRPC()
    {
        Effect[0].SetActive(true);
        Effect[1].SetActive(false);
    }
    [PunRPC]
    void EffectOffRPC()
    {
        Effect[1].SetActive(true);
        Effect[0].SetActive(false);
    }
    [PunRPC]
    void EffectAllOffRPC()
    {
        Effect[1].SetActive(false);
        Effect[0].SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
     if(OK &&other.CompareTag("Player") && !other.GetComponent<Move>().PV.IsMine)
        {
            other.GetComponent<BackMove>().PV.RPC("ObjMoveback4RPC", RpcTarget.All, transform.position.x, transform.position.y, transform.position.z,1000f + ChargeGage.fillAmount*1000f);
            if (FullCharge)
                Attack();
            else
                FullAttack();
        }
    }

    void Attack()
    {
        Effect[2].transform.position = transform.position;
        Effect[2].GetComponent<ParticleSystem>().Play();
    }
    void FullAttack()
    {
        Effect[3].transform.position = transform.position;
        Effect[3].GetComponent<ParticleSystem>().Play();
    }

    public void OKTrue()
    {
        OK = true;
    }

    public void OKFalse()
    {
        OK = false;
    }
}
