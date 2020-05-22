using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MachinegunOBJ : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    private Animator Ani;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        Ani = GetComponent<Animator>();
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
