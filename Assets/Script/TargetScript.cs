using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("B2"))
        {
            GetComponent<Rigidbody>().AddForce(other.transform.forward * 10000f);
            GameObject.Find("test").GetComponent<TutorialScript>().tutocheck2();

        }
    }
}
