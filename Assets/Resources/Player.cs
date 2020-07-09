using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Transform traf = null;
    
    // 속도
    public float moveSpeed = 0.3f;
    public float rotateSpeed = 0.3f;


    Vector3 rot = new Vector3(0, 0, 1);

    private void Start()
    {
        //초기화
        traf = transform;
        //gameManager = FindObjectOfType<GameManager>();

    }
    // Update is called once per frame
    void Update()
    {
        //움직임
        if (Input.GetKey(KeyCode.W))
        {
            traf.Translate(Vector3.forward * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            traf.Translate(Vector3.back * moveSpeed);

        }
        if (Input.GetKey(KeyCode.A))
        {
            traf.Translate(Vector3.left * moveSpeed);

        }
        if (Input.GetKey(KeyCode.D))
        {
            traf.Translate(Vector3.right * moveSpeed);

        }

        //카메라 조정
        if (Input.GetKey(KeyCode.Q))
        {
            traf.Rotate(0, rot.z * rotateSpeed, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.E))
        {
            traf.Rotate(0, -rot.z * rotateSpeed, 0, Space.World);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            traf.Rotate(rot.z * rotateSpeed, 0, 0);
        }
        if (Input.GetKey(KeyCode.C))
        {
            traf.Rotate(-rot.z * rotateSpeed, 0, 0);
        }

        if (Input.GetKey(KeyCode.X))
        {
            traf.position = new Vector3(0, 6, -6);
            traf.rotation = Quaternion.Euler(new Vector3(45, 0, 0));
        }
    }
}