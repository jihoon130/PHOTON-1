using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCol : MonoBehaviour
{
    public static CameraCol instance;

    // 카메라의 최소 거리
    public float minDistance = 1.0f;
    // 카메라의 최대 거리
    public float maxDistance = 4.0f;
    // 카메라 거리 저장
    public float SaveDistance;
    // 속도
    public float smooth = 10.0f;

    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;

    public float ZoomMax;

    Color SaveColor;
    MeshRenderer SaveObj;

    private void Awake()
    {
        SaveDistance = maxDistance;
        instance = this;
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    void LateUpdate()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit) && SaveObj)
        {
            SaveObj = hit.collider.gameObject.GetComponent<MeshRenderer>();

            SaveColor = hit.collider.gameObject.GetComponent<MeshRenderer>().material.color;
            SaveColor.a = 0.1f;
            hit.collider.gameObject.GetComponent<MeshRenderer>().material.color = SaveColor;
            //distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
        }
        else if(SaveObj)
        {
            SaveColor.a = 1.0f;
            SaveObj.material.color = SaveColor;
        }
            //distance = maxDistance;

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, smooth * Time.deltaTime);
    }

    public void CameraJoom(float joom)
    {
        maxDistance = joom;
    }
    public void CameraReset()
    {
        maxDistance = 4;
    }
}
