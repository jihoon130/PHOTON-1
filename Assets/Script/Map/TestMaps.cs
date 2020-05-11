using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class TestMaps : MonoBehaviourPunCallbacks
{

    [Serializable]
    public struct MapData
    {
        public GameObject[] MapObj;
        public int Min;
        public int Second;
        public float MapZ;
        public string Dir;
    }


    public MapData[] mapDatas;
    public PhotonView PV;

    private bool isCheckd;
    private int SaveIndex;

    private Timer timer;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        timer = GameObject.Find("TimerManger").GetComponent<Timer>();

    }
    void Start()
    {
        
    }

    void Update()
    {
        PV.RPC("TimerCheckMapRPC", RpcTarget.All);

        if(isCheckd)
        {
            if (SaveIndex == -1)
                return;

            PV.RPC("MapDownRPC", RpcTarget.All, SaveIndex);
        }
    }


    [PunRPC]
    void TimerCheckMapRPC()
    {
        for(int i = 0; i < mapDatas.Length; i++)
        {
            if (timer.Minute == mapDatas[i].Min && timer.Second == mapDatas[i].Second)
            {
                SaveIndex = i;
                isCheckd = true;
            }
        }
    }

    [PunRPC]
    void MapDownRPC(int index1)
    {
        for(int i = 0; i <= index1; i++)
        {
            for(int z = 0; z < mapDatas[index1].MapObj.Length; z++)
            {
                mapDatas[i].MapObj[z].transform.localPosition = new Vector3(0, 10, mapDatas[i].MapZ);

                if (mapDatas[i].Dir == "Down")
                {
                    mapDatas[i].MapZ -= 5 * Time.deltaTime;

                    if (mapDatas[i].MapZ < -14.2f)
                    {
                        mapDatas[i].MapZ = -14.2f;
                    }
                    SaveIndex = -1;
                    isCheckd = false;
                }
                else if (mapDatas[i].Dir == "Up")
                {
                    mapDatas[i].MapZ += 5 * Time.deltaTime;

                    if (mapDatas[i].MapZ > -5.5f)
                    {
                        mapDatas[i].MapZ = -5.5f;
                    }
                    SaveIndex = -1;
                    isCheckd = false;
                }
            }
            
        }
    }
}
