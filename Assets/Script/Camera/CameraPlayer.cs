using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public static CameraPlayer I;

    public static bool m_Cameara = true;

    // 카메라에 적용 할 타겟
    public Transform target;

    //타겟과 카메라와의 거리   
    public float dist = 3f;

    // 마우스 x,y 속도
    public float xSpeed = 180.0f;
    public float ySpeed = 30f;

    //카메라 초기 높이 설정
    public float InitRotY = 1.5f;

    //카메라 초기 위치
    private float x = 0.0f;
    private float y = 0.0f;

    //카메라 앵글 범위
    public float yMinLimit = -2f;
    public float yMaxLimit = 15f;

    private Quaternion rotation;
    private Vector3 position;

    public float CameraZ = 0.0f;

    private void Awake()
    {
        I = this;
    }

    void Start()
    {
        //커서 숨기기"//"를 지우세요
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {
        if (target)
        {
            //카메라 회전속도 계산
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

            // 카메라 앵글 제한두기
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            //카메라 위치 변화 계산
            rotation = Quaternion.Euler(y, x, 0);
            position = rotation * new Vector3(0.0f, 0.0f, -dist) + target.position + new Vector3(0.0f, InitRotY, CameraZ);

            // 계산된 값을 카메라 rotation에 적용
            transform.rotation = rotation;

            Vector3 vPos2 = new Vector3(position.x, position.y, position.z);
            transform.position = vPos2;

            target.Rotate(Vector3.up * Time.deltaTime * xSpeed * Input.GetAxis("Mouse X"));
        }
    }

    void Update()
    {

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
