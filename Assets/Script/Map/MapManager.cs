using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MapManager : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    private Timer timer;

    private bool isDown;

    public GameObject[] MapPattern_1, MapPattern_2, MapPattern_3, MapPattern_4,
                        MapPattern_5, MapPattern_5_1, MapPattern_6, MapPattern_7,
                        MapPattern_8, MapPattern_8_1, MapPattern_9, MapPattern_10;



    private int PatternType, UpPatternType;

    public float[] Z, Z2;

    private bool isCheckd;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        timer = GameObject.Find("TimerManger").GetComponent<Timer>();
    }
    private void Start()
    {
        
    }
    
    private void Update()
    {
        if (PatternType != 0)
            PV.RPC("MapArrayRPC", RpcTarget.All);

        if (MapCheck(2, 45))
            MapStart(1);
        else if (MapCheck(2, 35))
            MapStart(2);
        else if (MapCheck(2, 25))
            MapStart(3);
        else if (MapCheck(2, 10))
            MapStart(4);
        else if (MapCheck(2, 5))
            MapStart(5);
        else if (MapCheck(1, 50))
            MapStart(6);
        else if (MapCheck(1, 30))
            MapStart(7, 1);
        else if (MapCheck(1, 15))
            MapStart(8);
        else if (MapCheck(1, 10))
            MapStart(9);
        else if (MapCheck(0, 50))
            MapStart(10);
        else if (MapCheck(0, 35))
            MapStart(11, 2);
        else if (MapCheck(0, 20))
            MapStart(12, 3);
        else if (MapCheck(0, 15))
            MapStart(13);
    }
        
    void MapStart(int index, int up = 0) => PV.RPC("GetDownRPC", RpcTarget.All, index, up);

    private bool MapCheck(int min, int second)
    {
        if(timer.Minute == min && timer.Second == second)
            return true;
        return false;
    }

    [PunRPC]
    void MapArrayRPC()
    {
        // -11.2
        if (!isDown || PatternType == 0)
            return;

        if (PatternType == 1) MapCheckdDown(MapPattern_1, Z);
        else if (PatternType == 2) MapCheckdUp(MapPattern_1, Z, PatternType);
        else if (PatternType == 3) MapCheckdDown(MapPattern_2, Z);
        else if (PatternType == 4) MapCheckdUp(MapPattern_2, Z, PatternType);
        else if (PatternType == 5) MapCheckdDown(MapPattern_3, Z);
        else if (PatternType == 6) MapCheckdDown(MapPattern_4, Z);
        else if (PatternType == 7)
        {
            MapCheckdUp(MapPattern_5, Z2, UpPatternType);
            MapCheckdDown(MapPattern_5_1, Z);
        }
        else if (PatternType == 8) MapCheckdUp(MapPattern_5_1, Z, PatternType);
        else if (PatternType == 9) MapCheckdDown(MapPattern_6, Z);
        else if (PatternType == 10) MapCheckdDown(MapPattern_7, Z);
        else if (PatternType == 11)
        {
            MapCheckdUp(MapPattern_8, Z2, UpPatternType);
            MapCheckdDown(MapPattern_8_1, Z);
        }
        else if (PatternType == 12) MapCheckdUp(MapPattern_9, Z2, UpPatternType);
        else if (PatternType == 13) MapCheckdDown(MapPattern_10, Z);
    }

    void MapCheckdDown(GameObject[] obj, float[] z)
    {
        Debug.Log("PatternType : " + PatternType);
        if (PatternType == 0)
            return;

        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].transform.localPosition = new Vector3(0, 10, z[PatternType - 1]);
            z[PatternType - 1] -= 10 * Time.deltaTime;

            if (z[PatternType - 1] < -14.2f)
            {
                z[PatternType - 1] = -14.2f;

                if (i == obj.Length - 1)
                {
                    PV.RPC("GetUpRPC", RpcTarget.All);
                }
            }
        }
    }

    void MapCheckdUp(GameObject[] obj, float[] z, int index)
    {
        if (PatternType == 0)
            return;

        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].transform.localPosition = new Vector3(0, 10, z[index - 1]);
            z[index - 1] += 5 * Time.deltaTime;

            if (z[index - 1] > -5.5f)
            {
                z[index - 1] = -5.5f;
                if (i == obj.Length - 1)
                {
                    PV.RPC("GetUpRPC", RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    void GetDownRPC(int pattern, int up)
    {
        PatternType = pattern;
        UpPatternType = up;
        isDown = true;
    }

    [PunRPC]
    void GetUpRPC()
    {
        PatternType = 0;
        UpPatternType = 0;
        isDown = false;
    }
        
}
