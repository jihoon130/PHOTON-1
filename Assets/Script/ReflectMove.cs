using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectMove : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ObjMoveback3(collision);
        }
    }

    private void ObjMoveback3(Collision collision, float speed = 30.0f)
    {

        Vector3 pushdi = collision.transform.position - transform.position;
        pushdi = -pushdi.normalized;
    }
}
