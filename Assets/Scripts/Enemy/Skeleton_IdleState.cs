using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_IdleState : Skeleton_GroundedState
{

    public Skeleton_IdleState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName, Enemy_Skeleton _skeleton) : base(stateMachine, enemy, animBoolName , _skeleton)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = _skeleton.idleTime;
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
