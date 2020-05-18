using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class MapManager : MonoBehaviourPunCallbacks
{

    [Serializable]
    public struct MapData
    {
        public GameObject[] MapObj;
        public int Min;
        public int Second;
        public float MapZ;
        public string Dir;

        [HideInInspector]
        public bool isCheck;
    }


    public MapData[] mapDatas;
    public PhotonView PV;

    private bool isCheckd;

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
        TimerCheckMapRPC();

        if(isCheckd)
            MapDownRPC();
    }


    void TimerCheckMapRPC()
    {
        for(int i = 0; i < mapDatas.Length; i++)
        {
            if (timer.Minute == mapDatas[i].Min && timer.Second == mapDatas[i].Second)
            {
                mapDatas[i].isCheck = true;
                isCheckd = true;
            }
        }
    }

    void MapDownRPC()
    {
        for(int i = 0; i < mapDatas.Length; i++)
        {
            if (!mapDatas[i].isCheck)
                return;

            for (int z = 0; z < mapDatas[i].MapObj.Length; z++)
            {
                mapDatas[i].MapObj[z].transform.localPosition = new Vector3(0, 0, mapDatas[i].MapZ);

                if (mapDatas[i].Dir == "Down")
                {
                    mapDatas[i].MapZ -= 5 * Time.deltaTime;

                    if (mapDatas[i].MapZ < -8f)
                        mapDatas[i].MapZ = -8f;
                }
                else if (mapDatas[i].Dir == "Up")
                {
                    mapDatas[i].MapZ += 5 * Time.deltaTime;

                    if (mapDatas[i].MapZ > 0)
                        mapDatas[i].MapZ = 0;
                }
            }
        }
    }
}
