﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public struct Bullet
{

    public string BulletName { get; set; }
    public int BulletMany { get; set; }
    public int MinBullet { get; set; }
    public int MaxBullet { get; set; }
    public bool isBullet { get; set; }

    public Bullet(string name, int many, int min, int max, bool check = true)
    {
        this.BulletName = name;
        this.BulletMany = many;
        this.MinBullet = min;
        this.MaxBullet = max;
        this.isBullet = check;
    }
};

public class BulletManager : MonoBehaviourPunCallbacks
{
    private Create _Create;
    public PhotonView PV;
    public enum BulletMode { Shot, Machinegun, Grenade }
    public BulletMode _BulletMode = BulletMode.Shot;

    public Text MinText, MaxText;


    public Bullet[] BulletList = new Bullet[3];

    public GameObject _AimUi;
    public int type23;
    public static BulletManager I;

    private bool isUiScal;
    private float fScalXY = 1.5f;

    private AimS Aims;

    private string ModeName;
    private void Awake()
    {
        I = this;

        PV = GetComponent<PhotonView>();
        _Create = GetComponent<Create>();
        Aims = GetComponentInChildren<AimS>();

        MinText = GameObject.Find("Min").GetComponent<Text>();
        MaxText = GameObject.Find("Max").GetComponent<Text>();

        if (PV.IsMine)
        {
            BulletList[0] = new Bullet("Attack", 99, 99, 99);
            BulletList[1] = new Bullet("Machinegun", 20, 20, 100);
            BulletList[2] = new Bullet("Grenade", 5, 5, 0);
        }
    }
    void Start()
    {

    }
    void Update()
    {
        if (GetComponent<Move>().PV.IsMine)
        {
            if (GetComponent<Move>().StopT <= 0.0f)
            {
                BulletCheck();
                UITextUpdate();
            }
        }
    }
    void BulletCheck()
    {
        for (int i = 0; i < BulletList.Length; i++)
        {
            if (BulletList[i].MinBullet <= 0)
            {
                Aims.AimAttack(false);
                BulletList[i].isBullet = false;
                BulletList[i].MinBullet = 0;
            }
        }
    }

    public void BulletListAdd(int array, int min, int max)
    {
        BulletList[array].MinBullet = min;
        BulletList[array].MaxBullet = max;
        BulletList[array].isBullet = true;
    }

    public void BulletUse(int type)
    {
        if (_BulletMode == BulletMode.Shot)
            return;

        BulletList[type].MinBullet--;
    }

    public void BulletAdd(int type)
    {
        if (_BulletMode == BulletMode.Shot)
            return;

        int ManyNumber = BulletList[type].BulletMany - BulletList[type].MinBullet;
        int MaxCheck = BulletList[type].MaxBullet - ManyNumber;

        if (MaxCheck == 0 && BulletList[type].MinBullet < 0)
        {
            if (_BulletMode == BulletMode.Machinegun)
                GetComponent<Machinegun>().MachineDeleteReset();
            return;
        }
        else if (MaxCheck < 0)
        {
            Debug.Log(BulletList[type].MaxBullet);
            ManyNumber = BulletList[type].MaxBullet;
        }


        BulletList[type].MaxBullet -= ManyNumber;
        BulletList[type].MinBullet += ManyNumber;
        BulletList[type].isBullet = true;
    }
    public bool MaxBulletCheck(int type)
    {
        int nMaxBulletCheck = int.Parse(MaxText.text);

        if (nMaxBulletCheck <= 0)
            return true;
        return false;
    }

    public void UITextUpdate()
    {
        //if ((int)_BulletMode == 0) ModeName = "단발모드";

        int type = (int)_Create._BulletMake - 1;
        MinText.text = BulletList[type].MinBullet.ToString();
        MaxText.text = BulletList[type].MaxBullet.ToString();

        if (BulletList[1].MinBullet == 10)
            MinUiReset();

        if (BulletList[1].MinBullet <= 10)
        {
            if (!isUiScal)
                isUiScal = true;

            MinText.color = new Color(1, 0.3294118f, 0);
        }
        else if (BulletList[1].MinBullet >= 11)
            MinText.color = new Color(238, 235, 222);

        if (isUiScal)
        {
            MinText.transform.localScale = new Vector3(fScalXY, fScalXY, 0);
            fScalXY -= 2 * Time.deltaTime;

            if (fScalXY <= 1f)
                fScalXY = 1f;
        }
        //NameText.text = BulletList[type].BulletName.ToString();
        //ModeText.text = ModeName;
    }
    public void MinUiReset()
    {
        isUiScal = false;
        fScalXY = 1.5f;
    }

    public bool isGetItemCheck()
    {
        if (GetComponent<Machinegun>().isMachinegun || GetComponent<Grenade>().isGreande ||
            _BulletMode != BulletMode.Shot)
            return true;
        return false;
    }
}
