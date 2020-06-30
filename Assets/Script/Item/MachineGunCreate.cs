using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MachineGunCreate : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    private Timer timer;
    private bool isCreate;

    public GameObject RSpawn;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        timer = GameObject.Find("TimerManger").GetComponent<Timer>();
        RSpawn = GameObject.Find("MachineGunSpawn");
    }
    private void Start()
    {
        //InvokeRepeating("CheckUpdate", 0f, 0.5f);
    }
    void Update()
    {
        if (!PV.IsMine)
            return;

        if (timer.TimerCheck(2, 55) || timer.TimerCheck(2, 25) || timer.TimerCheck(2, 10) ||
            timer.TimerCheck(1, 50) || timer.TimerCheck(1, 30) || timer.TimerCheck(1, 1) ||
            timer.TimerCheck(0, 45) || timer.TimerCheck(0, 30) || timer.TimerCheck(0, 15))
        {
            if (isCreate)
                return;
        
            Create();
        }

    }

    private void CheckUpdate()
    {
        GameObject[] PlayerObj = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < PlayerObj.Length; i++)
        {
            PlayerObj[i] = GameObject.FindGameObjectWithTag("Player");

            if (PlayerObj[i].GetComponent<Machinegun>().isMachineAttack ||
                PlayerObj[i].GetComponent<Machinegun>().isMachineRay ||
                PlayerObj[i].GetComponent<Machinegun>().isMachinegun)
            {
                Debug.Log(i + "번째 플레이어 무기있음");
                return;
            }

            Debug.Log("현재 무기 아무도없음");
        }
    }


    private void Create()
    {
        if (RSpawn.GetComponent<Respawn>().f == false)
            RSpawn.GetComponent<Respawn>().a = false;

        Vector3 spawn = new Vector3(RSpawn.transform.position.x, RSpawn.transform.position.y + 2f, RSpawn.transform.position.z);
        PhotonNetwork.Instantiate("Machine_gun_Item", spawn, Quaternion.Euler(90, 0, 0));

        RSpawn.GetComponent<Respawn>().RePosition();
        StartCoroutine("TimerCreate");
        PV.RPC("CreateCheck", RpcTarget.All, true);
    }

    IEnumerator TimerCreate()
    {
        yield return new WaitForSeconds(5f);
        PV.RPC("CreateCheck", RpcTarget.All, false);
    }

    [PunRPC]
    private void CreateCheck(bool check) => isCreate = check;
}
