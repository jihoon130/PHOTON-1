using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MachinegunOBJ : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    private Animator Ani;

    public ParticleSystem[] effect;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        Ani = GetComponent<Animator>();

        for (int i = 0; i < effect.Length; i++)
            effect[i].Stop();
    }
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void AttackChang(bool attack)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Machinegun>().isMachineRay = attack;
        Ani.SetBool("Attack", attack);
    }
}
