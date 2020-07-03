using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step1 : MonoBehaviour
{
    public GameObject Timer;

    private void OnEnable()
    {
        Timer.SetActive(true);
    }
}
