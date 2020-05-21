using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MachinegunOBJ : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    private Animator Ani;
    private BulletManager _BulletManager;

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

    public void AttackChang(bool attack) => Ani.SetBool("Attack", attack);
}
