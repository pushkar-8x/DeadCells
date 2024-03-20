using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_AttackState : EnemyState
{
    private Enemy_Skeleton _skeleton;
    public Skeleton_AttackState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName, Enemy_Skeleton _skeleton) : base(stateMachine, enemy, animBoolName)
    {
        this._skeleton = _skeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        _skeleton.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        _skeleton.SetZeroVelocity();
        if(triggerCalled)
        {
            stateMachine.SwitchState(_skeleton._battleState);
        }
    }
}
