using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CastMove : MonoBehaviourPunCallbacks
{
    public float CastSpeed = 50.0f;
    public PhotonView PV;
    public bool a=false;
    public Vector3 vf;
    Vector3 reflectvector; float angle;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        reflectvector = Vector3.zero;
    }

    void Update()
    {
       

    }

    private void FixedUpdate()
    {
        if (gameObject.CompareTag("ReflectBullet"))
        {

            RaycastHit hit;
            Vector3 pos = transform.position;
            Vector3 dir = transform.forward;
            if (Physics.Raycast(pos, dir, out hit, 100f))
            {
                pos = hit.point;
                reflectvector = Vector3.Reflect(dir, hit.normal);
                angle = Mathf.Atan2(reflectvector.x, reflectvector.z) * Mathf.Rad2Deg;
            }
        }

        transform.Translate(Vector3.forward * Time.deltaTime * CastSpeed);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(gameObject.CompareTag("ReflectBullet") && collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("shield"))
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, angle, transform.rotation.z));
        }
    }

    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);
}
