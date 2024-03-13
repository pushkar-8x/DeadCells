using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_MoveState : EnemyState
{
    private Enemy_Skeleton _skeleton;
    public Skeleton_MoveState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName, Enemy_Skeleton skeleton) : base(stateMachine, enemy, animBoolName)
    {
        _skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
