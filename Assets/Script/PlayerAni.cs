using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State
{
    IdleRun,
    Attack
}
public class PlayerAni : MonoBehaviour
{
    private Animator Ani;
    private Move _Move;

    public State _State = State.IdleRun;

    void Start()
    {
        Ani = GetComponent<Animator>();
        _Move = GetComponent<Move>();
    }

    void Update()
    {
        PlayerStateCheck();
    }


    void PlayerStateCheck()
    {
        switch (_State)
        {
            case State.IdleRun:
                {
                    AniChange(0);
                    MoveMent();
                }
                break;
            case State.Attack:
                {
                    AniChange(1);
                }
                break;
        }
    }
    public void AniChange(int AniType)
    {
        Ani.SetInteger("State", AniType);
    }

    public void MoveMent()
    {
        float horizontal = _Move.fHorizontal;
        float vertical = _Move.fVertical;

        Ani.SetFloat("MoveSpeed", horizontal * horizontal + vertical * vertical, 0.1f, Time.deltaTime);
    }
}
