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

    private bool isAttack = true;

    private bool isPunchM2; // 차징모으는사운드

    private void Awake()
    {

        Audio = GetComponent<AudioSource>();
        _PlayerAni = GetComponentInParent<PlayerAni>();
        _Move = GetComponentInParent<Move>();
        Timers = GameObject.Find("TimerManger").GetComponent<Timer>();

        Punch_SoundStop();


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
            isPunchM2 = true;
            _Move.MoveSpeed = 2.5f;

            if (ChargeGage.fillAmount <= 1f)
            {
                ChargeGage.fillAmount += Time.deltaTime;
            }
        }
        if (Input.GetMouseButtonUp(1) && isPunchM2)
        {
            Punch_SoundStop();
            _Move.MoveSpeed = 5.0f;
            if (!OK)
            {
                CameraCol.instance.CameraReset();
                ChargeGage.fillAmount = 0f;
            }
            pv.RPC("EffectAllOffRPC", RpcTarget.All);
            isPunchM2 = false;
        }

        if (Input.GetMouseButtonDown(0) && isAttack)
        {
            CameraCol.instance.CameraReset();
            _PlayerAni._State = State.Attack;
        }


        if (_PlayerAni._State == State.Dash || _PlayerAni._State == State.Dmg)
        {
            OK = false;
            _Move.MoveSpeed = 5.0f;
            ChargeGage.fillAmount = 0f;
        }

        if(ChargeGage.fillAmount<=0.9f)
            FullCharge = false;

        if (!OK&&!Effect[0].activeInHierarchy &&ChargeGage.fillAmount >= 0.1f && ChargeGage.fillAmount <= 0.9f)
        {
            SoundPlayer(2);
            pv.RPC("EffectOnRPC", RpcTarget.All);
        }
        else if(!OK && !Effect[1].activeInHierarchy && ChargeGage.fillAmount >= 1f)
        {
            SoundPlayer(3);
            CameraCol.instance.CameraJoom(1.5f);
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
            SoundPlayer(Random.Range(0, 2));

            
            if (!FullCharge)
            {
                other.GetComponent<BackMove>().PV.RPC("ObjMoveback4RPC", RpcTarget.All, _Move.gameObject.transform.position.x, _Move.gameObject.transform.position.y, _Move.gameObject.transform.position.z, 10f + (ChargeGage.fillAmount * 100f), pv.Owner.ToString());
                Attack();
            }
            else
            {
                other.GetComponent<BackMove>().PV.RPC("ObjMoveback5RPC", RpcTarget.All, _Move.gameObject.transform.position.x, 0.001f, _Move.gameObject.transform.position.z, 200f, pv.Owner.ToString());
                FullAttack();
            }
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

    public void SoundPlayer(int type)
    {
        if (!Timers.isStart)
            return;

        Audio.clip = audios[type];
        Audio.Play();
    }
    private void Punch_SoundStop()
    {
        Audio.clip = audios[2];
        Audio.Stop();
    }
}
