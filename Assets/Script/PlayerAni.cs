using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State
{
    IdleRun,
    Attack,    
    Dmg,
    Jump_Start,
    Jump_Ing,
    Jump_End,
    Dash,
    Machinegun,
    Greande
}
public class PlayerAni : MonoBehaviour
{
    [HideInInspector]
    public Animator Ani;
    private Move _Move;

    public State _State = State.IdleRun;

    public float CurAniTime;

    void Start()
    {
       
        Ani = GetComponent<Animator>();
        _Move = GetComponent<Move>();
    }

    void Update()
    {

        PlayerStateCheck();

        AttackMent(_Move.fHorizontal, "VelocityX");
        AttackMent(_Move.fVertical, "VelocityZ");

        if (_State != State.Machinegun || _State != State.Greande)
        {
            if (Ani.GetCurrentAnimatorStateInfo(0).normalizedTime < CurAniTime)
            {
                _State = State.IdleRun;
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            //Ani.SetLayerWeight(1, 1.0f);
            _State = State.Machinegun;
        }
            

    }


    void PlayerStateCheck()
    {
        switch (_State)
        {
            case State.IdleRun:
                {
                    AniChange(0);
                    //MoveMent(_Move.fHorizontal, _Move.fVertical, "MoveSpeed");
                }
                break;
            case State.Attack:
                {
                    AniChange(1);
                }
                break;
            case State.Dmg:
                {
                    AniChange(3);
                }
                break;

            case State.Jump_Start:
                {
                    AniChange(10);
                }
                break;
            case State.Jump_Ing:
                {
                    AniChange(11);
                }
                break;
            case State.Jump_End:
                {
                    AniChange(12);
                }
                break;
            case State.Dash:
                {
                    AniChange(13);
                }
                break;
            case State.Machinegun:
                {
                    AniChange(14);
                }
                break;
            case State.Greande:
                {
                    AniChange(15);
                }
                break;
        }
    }
    public void AniChange(int AniType)
    {

        Ani.SetInteger("State", AniType);
    }

    public void MoveMent(float h, float v, string name)
    {
        float horizontal = h;
        float vertical = v;
        Ani.SetFloat(name, horizontal * horizontal + vertical * vertical, 0.1f, Time.deltaTime);
    }

    public void AttackMent(float Dir, string name)
    {
        float dir = Dir;
        Ani.SetFloat(name, dir, 0.1f, Time.deltaTime);
    }
}
