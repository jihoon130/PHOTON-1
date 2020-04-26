using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public static CameraPlayer I;

    public static bool m_Cameara = true;
    public Transform target;

    //카메라와의 거리   
    public float dist = 3f;

    public float xSpeed = 180.0f;
    public float ySpeed = 60.0f;

    //카메라 초기 높이 설정
    public float InitRotY = 1.5f;

    //카메라 초기 위치
    private float x = 0.0f;
    private float y = 0.0f;

    public float yMinLimit = -5f;
    public float yMaxLimit = 30f;

    private Quaternion rotation;
    private Vector3 position;


    private void Awake()
    {
        I = this;
    }
    void Start()
    {
        //커서 숨기기"//"를 지우세요
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {
        if (target)
        {
            //마우스 스크롤과의 거리계산
            //dist -= ScrollSpeed * Input.mouseScrollDelta.y * Time.deltaTime;
            //
            ////마우스 스크롤했을경우 카메라 거리의 Min과Max
            //if (dist < -3.0)
            //{
            //    dist = 3.0f;
            //}
            //if (dist >= 0) dist = 0;

            //카메라 회전속도 계산
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

            //앵글값 정하기
            //y값의 Min과 MaX 없애면 y값이 360도 계속 돎
            //x값은 계속 돌고 y값만 제한
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            //카메라 위치 변화 계산
            rotation = Quaternion.Euler(y, x, 0);
            position = rotation * new Vector3(0, 0.0f, -dist) + target.position + new Vector3(0.0f, InitRotY, 0.0f);

            transform.rotation = rotation;
            transform.position = position;
            //transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, 3 * Time.deltaTime);

            target.Rotate(Vector3.up * Time.deltaTime * xSpeed * Input.GetAxis("Mouse X"));
        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
