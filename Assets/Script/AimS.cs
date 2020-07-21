using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AimS : MonoBehaviour
{
    private Animator AimAni;
    Image sp;
    Move move;
    private Create _Create;

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
        AimAttack(false);
    }

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

        if (move.dieOk == true)
        {
            sp.color = new Color(255, 255, 255, 0);
            return;
        }
    }
    public void AimAttack(bool check) => AimAni.SetBool("Attack", check);
    public void AimState(int type) => AimAni.SetInteger("State", type);
}
