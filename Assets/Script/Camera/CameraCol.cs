using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCol : MonoBehaviour
{
    public static CameraCol instance;

    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float SaveDistance;
    public float smooth = 10.0f;

    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;

    public float ZoomMax;

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

        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
        {
            distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
        }
        else
            distance = maxDistance;

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
