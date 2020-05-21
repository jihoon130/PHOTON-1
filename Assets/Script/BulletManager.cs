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
    public enum BulletMode { Shot }
    public BulletMode _BulletMode = BulletMode.Shot;

    public Text MinText, MaxText, NameText, ModeText;


    public Bullet[] BulletList = new Bullet[3];

    public GameObject _AimUi;
    public int type23;
    public static BulletManager I;

    private string ModeName;
    private void Awake()
    {
        I = this;

        PV = GetComponent<PhotonView>();
        _Create = GetComponent<Create>();

        MinText = GameObject.Find("Min").GetComponent<Text>();
        MaxText = GameObject.Find("Max").GetComponent<Text>();
        NameText = GameObject.Find("Name").GetComponent<Text>();
        ModeText = GameObject.Find("Mode").GetComponent<Text>();

        if(PV.IsMine)
        {
            BulletList[0] = new Bullet("Attack", 30, 30, 90);
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
                BulletList[i].isBullet = false;
                BulletList[i].MinBullet = 0;
            }
        }
    }

    public void BulletUse(int type)
    {
        BulletList[type].MinBullet--;
    }

    public void BulletAdd(int type)
    {
        int ManyNumber = BulletList[type].BulletMany - BulletList[type].MinBullet;
        int MaxCheck = BulletList[type].MaxBullet - ManyNumber;

        if (MaxCheck < 0)
            return;

        BulletList[type].MaxBullet -= ManyNumber;
        BulletList[type].MinBullet += ManyNumber;
        BulletList[type].isBullet = true;
    }

    public void UITextUpdate()
    {
        if ((int)_BulletMode == 0) ModeName = "단발모드";

        int type = (int)_Create._BulletMake - 1;
        MinText.text = BulletList[type].MinBullet.ToString();
        MaxText.text = BulletList[type].MaxBullet.ToString();
        NameText.text = BulletList[type].BulletName.ToString();
        ModeText.text = ModeName;
    }
}
