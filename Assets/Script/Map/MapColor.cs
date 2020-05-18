using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

[Serializable]
public struct MapData
{
    public int Min;
    public int Second;
    public MeshRenderer[] obj;
}
public class MapColor : MonoBehaviourPunCallbacks
{
    public Material[] ChangeMt;

    public MapData[] Mapdata;
    public PhotonView PV;
    private Timer timer;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        timer = GameObject.Find("TimerManger").GetComponent<Timer>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimerCheckMaps();
    }

    void TimerCheckMaps()
    {
        for (int i = 0; i < Mapdata.Length; i++)
        {
            if (timer.Minute == Mapdata[i].Min && timer.Second == Mapdata[i].Second + 3)
            {
                for (int z = 0; z < Mapdata[i].obj.Length; z++)
                {
                    Mapdata[i].obj[z].material = ChangeMt[0];
                }
            }
            else if (timer.Minute == Mapdata[i].Min && timer.Second == Mapdata[i].Second)
            {
                for (int z = 0; z < Mapdata[i].obj.Length; z++)
                {
                    Mapdata[i].obj[z].material = ChangeMt[1];
                }
            }
        }
    }
}
