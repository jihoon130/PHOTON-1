using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GreandeTimer : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
public    Rigidbody rb;
    public float rs = 0f;
    bool a = false;
    float t = 5.0f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    void Start()
    {
        StartCoroutine("DestroyTimer");
    }

    private void Update()
    {
        

        if(rs != 0f && !a)
        {
            transform.rotation = Quaternion.Euler(0, rs, 0);
            if(t<=0.1f)
            {
                t = 0f;
            }
            if(t!=0f)
            t -= Time.deltaTime;
            transform.Translate(Vector3.forward * Time.deltaTime * t);
       
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
