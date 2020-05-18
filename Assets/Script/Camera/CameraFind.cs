using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFind : MonoBehaviour
{
    public static CameraFind _instance;

    [SerializeField]
    public GameObject CameraFollowObj;

    public float clampAngle = 80.0f;
    public float inputSensitivity = 120.0f; // 움직이는 속도값
    public float mouseX; // 현재 계속 받아오는 마우스X값
    public float mouseY; // 현재 계속 받아오는 마우스Y값
    public float finalInputX; // 마우스X값 저장
    public float finalInputZ; // 마우스Y값 저장
    private float rotY = 0.0f; // 카메라의 Y축값
    private float rotX = 0.0f; // 카메라의 X축값
    public bool islock;
    // 카메라의 로테이션 y값을 저장하기 위해 사용
    public Quaternion _CameraRight = Quaternion.identity;

    public float m_fLerpSpeed = 500.0f;

    public bool isLockOn; // 락온

    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
    void LateUpdate()
    {
        if (CameraFollowObj == null)
            return;

        if (!islock)
        {
            MouseRot();
            CameraUpdater();
        }
    }
    void MouseRot()
    {
        if (isLockOn)
            return;

        // 마우스 x, y값 받아오기
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        // 설정한 마우스의 값을 보내주기 
        finalInputX = mouseX;
        finalInputZ = mouseY;

        // 마우스정보 * 지정속도 -> rotY(카메라 로테이션)값에 계속 더해줌
        rotY += finalInputX * inputSensitivity * Time.deltaTime;
        rotX += finalInputZ * inputSensitivity * Time.deltaTime;

        // 카메라 각도 360' 회전하는걸 막아주며 지정한 값 범위 내에서만 확인가능
        rotX = Mathf.Clamp(rotX, -30, clampAngle);

        // 카메라 로테이션 돌려주기
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, localRotation, Time.deltaTime * 15);

        // 카메라 마우스 y값을 보간
        _CameraRight.eulerAngles = new Vector3(0, rotY, 0);
    }
    void CameraUpdater()
    {
        // 플레이어 중심에 설정한 오브젝트 좌표값을 받아옴
        Transform target = CameraFollowObj.transform;
        transform.position = Vector3.Lerp(transform.position, target.position, m_fLerpSpeed * Time.deltaTime);
    }
}
