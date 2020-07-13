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
    private UIPopUp popUp;

    public Material[] RedMat;
    public Material[] ChangeMt;
    

    public MapData[] Mapdata;
    public PhotonView PV;
    private Timer timer;

    private RobbySound _MapSound;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        timer = GameObject.Find("TimerManger").GetComponent<Timer>();
        popUp = GameObject.Find("Map_POPUP").GetComponent<UIPopUp>();

        _MapSound = GameObject.Find("UISoundManager").GetComponent<RobbySound>();
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

            if (timer.Minute == Mapdata[i].Min && timer.Second == Mapdata[i].Second + 5)
            {
                popUp.MoveXValue(890);
            }
            else if (timer.Minute == Mapdata[i].Min && timer.Second == Mapdata[i].Second + 3)
            {
                _MapSound.SoundPlayer(5);
                for (int z = 0; z < Mapdata[i].obj.Length; z++)
                {
                    Mapdata[i].obj[z].materials = RedMat;
                }
            }
            else if (timer.Minute == Mapdata[i].Min && timer.Second == Mapdata[i].Second)
            {
                popUp.MoveXValue(1300);
                for (int z = 0; z < Mapdata[i].obj.Length; z++)
                {
                    Mapdata[i].obj[z].materials = ChangeMt;
                }
            }
        }
    }
}
