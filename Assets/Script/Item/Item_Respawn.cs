using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Item_Respawn : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    private UIPopUp popUp;

    private List<Transform> ItemPos = new List<Transform>();

    public Transform[] SpawnPos;

    private Transform SetPosTf;

    public bool isSpawn;
    public bool isMatCheck;
    

    private Timer timer;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        timer = GameObject.Find("TimerManger").GetComponent<Timer>();
        popUp = GameObject.Find("Map_POPUP").GetComponent<UIPopUp>();
    }
    void Start()
    {
        ResetPos();
    }

    void Update()
    {
        if (timer.ItemMinute > 5 && !isSpawn)
        {
            if (isSpawn)
                return;

            popUp.MoveXValue2(1300);

            SetSpawnPos();
        }
        else if (timer.ItemMinute < 1)
        {
            isSpawn = false;
        }
        else if (timer.ItemMinute == 3)
        {
            popUp.MoveXValue2(890);
        }
    }

    private void SetSpawnPos()
    {
        if (!PV.IsMine)
            return;

        if (ItemPos.Count == 0) 
            ResetPos();

        int nRandom = Random.Range(0, ItemPos.Count);
        SetPosTf = ItemPos[nRandom];

        this.transform.position = SetPosTf.position;

        isMatCheck = true;

        DeleteList(SetPosTf);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isMatCheck)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                Debug.Log(collision.gameObject.GetComponent<MeshRenderer>().material.name);
                if (collision.gameObject.GetComponent<MeshRenderer>().material.name == "Red")
                {
                   SetSpawnPos();
                   return;
                }
                else
                {
                   CreateItem();
                }
            }
            else
                SetSpawnPos();
        }
    }

    private void CreateItem()
    {

        string ItemName = string.Empty;
        int nRandomItemRange = Random.Range(1, 3);

        if (nRandomItemRange == 1) ItemName = "Machine_gun_Item";
        else ItemName = "Grenade_Obj";


        PhotonNetwork.Instantiate(ItemName, new Vector3(this.transform.position.x,
                                                                  this.transform.position.y + 2f,
                                                                  this.transform.position.z),
                                                                  Quaternion.Euler(90, 0, 0));
        this.transform.position = new Vector3(100, -10f, 0);

        isMatCheck = false;
        isSpawn = true;
    }

    private void ResetPos()
    {
        for (int i = 0; i < SpawnPos.Length; i++)
        {
            ItemPos.Add(SpawnPos[i]);
        }
    }

    private void DeleteList(Transform Tf) => ItemPos.Remove(Tf);

}
