using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
     public bool a = true;
    public bool f=false;
    float b;
    float c;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!a)
        {
            b = Random.Range(-15f, 15f);
            c = Random.Range(-25f, 5f);
            transform.position = new Vector3(b, transform.position.y, c);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            a = true;
            f = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            a = false;
        }
    }

    public void RePosition()
    {
        transform.position = new Vector3(0.26f, 5.057f, -9.12f);
        f = false;
    }
}
