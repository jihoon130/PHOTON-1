using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AimS : MonoBehaviour
{
    private Vector3 ScreenCenter;
    RaycastHit hit;
    Image sp;
    public float y;
    // Start is called before the first frame update
    private void Awake()
    {
        if (!transform.root.GetComponent<Move>().PV.IsMine)
            gameObject.SetActive(false);
        sp = GetComponent<Image>();
    }
    void Start()
    {
        y = 400.0f;
        ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
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


       if (y <= 550f && y >= 336f)
            y += Input.GetAxis("Mouse Y") * 500.0f * Time.deltaTime;

       y = Mathf.Clamp(y, 336f, 550f);

        transform.position = new Vector3(transform.position.x, y, transform.position.z);
        ScreenCenter.y = y;
        Ray ray = Camera.main.ScreenPointToRay(ScreenCenter);
        if(Physics.Raycast(ray,out hit))
        {
            if (hit.collider.name == "Player(Clone)" && !hit.collider.gameObject.GetComponent<Move>().PV.IsMine)
            {
                sp.color = new Color(255, 0, 0);
            }
            else
            {
                sp.color = new Color(255, 255, 255);
            }
        }
    }
}
