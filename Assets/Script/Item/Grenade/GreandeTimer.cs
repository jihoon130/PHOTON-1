using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GreandeTimer : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    Rigidbody rb;

    void Start()
    {
        StartCoroutine("DestroyTimer");
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //rb.AddForce(transform.forward * 15, ForceMode.Impulse);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int floorMask = LayerMask.GetMask("Ground");
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500f,floorMask))
        {
            Vector3 playerToMouse = hit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRot = Quaternion.LookRotation(playerToMouse);
            rb.MoveRotation(newRot);
        }

    }
    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(3f);
        ObjectDestroys();
    }

    public void ObjectDestroys()
    {
        PhotonNetwork.Instantiate("Grenade_Boom", this.transform.position, Quaternion.identity);
        GetComponent<ItemDestroy>().PV.RPC("DestroyRPC", Photon.Pun.RpcTarget.All);
    }

}
