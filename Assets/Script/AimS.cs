using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AimS : MonoBehaviour
{
    private Animator AimAni;

    private Vector3 ScreenCenter;
    RaycastHit hit;
    Image sp;
    Move move;
    private Create _Create;
    public float y;

    // Start is called before the first frame update
    private void Awake()
    {
        if (!transform.root.GetComponent<Move>().PV.IsMine)
            gameObject.SetActive(false);
        sp = GetComponent<Image>();
        move = GetComponentInParent<Move>();
        _Create = GetComponentInParent<Create>();
        AimAni = GetComponent<Animator>();
    }
    void Start()
    {
        y = 400.0f;
        ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        AimAttack(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("TimerManger").GetComponent<Timer>().isStart)
            GetComponent<Image>().enabled = true;
        else
            GetComponent<Image>().enabled = false;

        if (GameObject.Find("EndScore"))
        {
            Destroy(this.gameObject);
        }

        //if (move.dieOk == false)
        //{
        //    if (y <= 550f && y >= 336f)
        //        y += Input.GetAxis("Mouse Y") * 500.0f * Time.deltaTime;

        //    y = Mathf.Clamp(y, 336f, 550f);
        //}

        //transform.position = new Vector3(transform.position.x, y, transform.position.z);
        //ScreenCenter.y = y;

        if (move.dieOk == true)
        {
            sp.color = new Color(255, 255, 255, 0);
            return;
        }
    }
    public void AimAttack(bool check) => AimAni.SetBool("Attack", check);
    public void AimState(int type) => AimAni.SetInteger("State", type);
}
