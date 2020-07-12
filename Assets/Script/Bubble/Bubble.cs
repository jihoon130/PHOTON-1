using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bubble : MonoBehaviour
{
    private Move _Move;

    ParticleSystem pa;
    bool a;
    // Start is called before the first frame update
    private void Awake()
    {
        pa = GetComponent<ParticleSystem>();
        _Move = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pa.particleCount >= 1)
        {
            a = true;
        }

        if (a && pa.particleCount==0)
        {
            _Move.isMove = true;
            a = false;
            gameObject.SetActive(false);
        }
    }
}
