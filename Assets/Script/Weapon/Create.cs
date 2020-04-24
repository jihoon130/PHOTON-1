using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Create : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public Transform StartTf;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            InstantiateObject("CastObj_1", StartTf.transform.position, RotVector());
        }
    }

    private void InstantiateObject(string objname, Vector3 vStartPos, Vector3 vStartRot)
    {
        PhotonNetwork.Instantiate(objname, vStartPos, Quaternion.Euler(vStartRot));
    }

    private Vector3 RotVector(float z = 90f)
    {
        Vector3 Rot;
        Rot.x = transform.rotation.eulerAngles.x;
        Rot.y = transform.rotation.eulerAngles.y;
        Rot.z = z;

        return Rot;
    }
}
