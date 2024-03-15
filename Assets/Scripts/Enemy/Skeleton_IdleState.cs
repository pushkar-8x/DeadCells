using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_IdleState : EnemyState
{
    private Enemy_Skeleton _skeleton;
    public Skeleton_IdleState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName, Enemy_Skeleton skeleton) : base(stateMachine, enemy, animBoolName)
    {
        _skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 2f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
        {
            stateMachine.SwitchState(_skeleton._moveState);
        }
    }
}
