using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake camerashake;

    public float shakes = 0f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    public Vector3 originalPos;
    public GameObject ShakeObj;
    bool CameraShaking;


    private void Awake()
    {
        camerashake = this;
    }

    void Start()
    {
        CameraShaking = false;
    }
    public void Init()
    {
        
        originalPos = ShakeObj.transform.position;
    }
    public void ShakeCamera()
    {
        //gameObject.GetComponent<Came>().islock = true;
        shakes = 0.1f;
        originalPos = ShakeObj.transform.position;
        CameraShaking = true;
    }

    void FixedUpdate()
    {
        if (CameraShaking)
        {
            if (shakes > 0)
            {
                transform.position = ShakeObj.transform.position + Random.insideUnitSphere * shakeAmount;
                shakes -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakes = 0f;
                gameObject.transform.position = originalPos;
                CameraShaking = false;
            }
        }
    }
}
