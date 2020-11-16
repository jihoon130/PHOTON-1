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
    public Image ChargeGage;
    public PlayerAni _PlayerAni;
    public Move _Move;
    public GameObject[] Effect;
    bool FullCharge=false;


    private Timer Timers;

    AudioSource Audio;
    public AudioClip[] audios;

    private void Awake()
    {
        Audio = GetComponent<AudioSource>();
        _PlayerAni = GetComponentInParent<PlayerAni>();
        _Move = GetComponentInParent<Move>();
        Timers = GameObject.Find("TimerManger").GetComponent<Timer>();

        ChargeGage = GameObject.Find("ChargeGage").GetComponent<Image>();
        Effect[2] = GameObject.Find("hit");
        Effect[3] = GameObject.Find("charge_hit");
    }


    private void Update()
    {
        if (!pv.IsMine)
            return;

        if (!_Move.isSpawnAttack)
            return;

        if (!OK&&Input.GetMouseButton(1) && !_Move.dieOk && _PlayerAni._State != State.Dash && _PlayerAni._State != State.Dmg)
        {
            _Move.MoveSpeed = 2.5f;

            if (ChargeGage.fillAmount <= 1f)
                ChargeGage.fillAmount += Time.deltaTime/3;
        }
        if (Input.GetMouseButtonUp(1))
        {
            _Move.MoveSpeed = 5.0f;
            if(!OK)
            ChargeGage.fillAmount = 0f;
            FullCharge = false;

         pv.RPC("EffectAllOffRPC", RpcTarget.All);
        }

        if (Input.GetMouseButtonDown(0))
        {
             _PlayerAni._State = State.Attack;
        }

        if (_PlayerAni._State == State.Dash || _PlayerAni._State == State.Dmg)
        {
            OK = false;
            _Move.MoveSpeed = 5.0f;
            ChargeGage.fillAmount = 0f;
        }

        if(!OK&&!Effect[0].activeInHierarchy &&ChargeGage.fillAmount >= 0.1f && ChargeGage.fillAmount <= 0.9f)
        {
            pv.RPC("EffectOnRPC", RpcTarget.All);
        }
        else if(!OK && !Effect[1].activeInHierarchy && ChargeGage.fillAmount >= 1f)
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
            SoundPlayer(0);

            other.GetComponent<BackMove>().PV.RPC("ObjMoveback4RPC", RpcTarget.All, transform.position.x, transform.position.y, transform.position.z, 10f + ChargeGage.fillAmount * 10f, pv.Owner.ToString());
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

    private void SoundPlayer(int type)
    {
        if (!Timers.isStart)
            return;

        Audio.clip = audios[type];
        Audio.Play();
    }
}
