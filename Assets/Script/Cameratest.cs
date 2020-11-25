using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameratest : MonoBehaviour
{
    public Vector3 pos;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        int floorMask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Wall");
        if (Physics.Raycast(transform.position,transform.forward,out hit,500f,floorMask))
        {
            pos = hit.point;
        }

    }
}
