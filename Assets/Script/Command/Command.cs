using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : MonoBehaviour
{
    public AimS Aim;
    public BulletManager Bulletmanager;
    public Machinegun Machineguns;

    private void Awake()
    {
        Aim = GetComponentInChildren<AimS>();
        Bulletmanager = GetComponent<BulletManager>();
        Machineguns = GetComponent<Machinegun>();
    }

}
