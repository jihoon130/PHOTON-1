using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    ParticleSystem pa;
    bool a;
    // Start is called before the first frame update
    private void Awake()
    {
        pa = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pa.particleCount >= 1)
            a = true;

        if(a && pa.particleCount==0)
        {
            a = false;
            gameObject.SetActive(false);
        }
    }
}
