using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Bullet
{
    public string BulletName;
    public int BulletMany;
    public int MinBullet;
    public int MaxBullet;
    public bool isBullet;

    public Bullet(string name, int many, int min, int max, bool check = true)
    {
        this.BulletName = name;
        this.BulletMany = many;
        this.MinBullet = min;
        this.MaxBullet = max;
        this.isBullet = check;
    }
};

public class BulletManager : MonoBehaviour
{
    private Create _Create;

    public enum BulletMode { Shot, Speaker }
    public BulletMode _BulletMode = BulletMode.Shot;

    public Text MinText, MaxText, NameText, ModeText;

    public Bullet[] BulletList = new Bullet[]
    {
        new Bullet ("Attack",30, 30, 999),
        new Bullet ("Speed",3, 3 ,30)
    };

    public static BulletManager I;

    private string ModeName;
    private void Awake()
    {
        I = this;

        _Create = GetComponent<Create>();

        MinText = GameObject.Find("Min").GetComponent<Text>();
        MaxText = GameObject.Find("Max").GetComponent<Text>();
        NameText = GameObject.Find("Name").GetComponent<Text>();
        ModeText = GameObject.Find("Mode").GetComponent<Text>();
    }
    void Start()
    {
    }
    void Update()
    {
        BulletCheck();
        UITextUpdate();
    }
    void BulletCheck()
    {
        for(int i = 0; i < BulletList.Length; i++)
        {
            if(BulletList[i].MinBullet <= 0)
            {
                BulletList[i].isBullet  = false;
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
        if(Input.GetMouseButtonDown(1))
        {
            if (_BulletMode == BulletMode.Speaker)
                _BulletMode = BulletMode.Shot;
            else
                _BulletMode++;
        }

        if ((int)_BulletMode == 0) ModeName = "단발모드"; 
        else if ((int)_BulletMode == 1) ModeName = "연사모드"; 

        int type = (int)_Create._BulletMake - 1;

        MinText.text  = BulletList[type].MinBullet.ToString();
        MaxText.text  = BulletList[type].MaxBullet.ToString();
        NameText.text = BulletList[type].BulletName.ToString();
        ModeText.text = ModeName;
    }

}
