using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_MoveState : Skeleton_GroundedState
{
    public Skeleton_MoveState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName, Enemy_Skeleton _skeleton) : base(stateMachine, enemy, animBoolName, _skeleton)
    {
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

        _skeleton.SetVelocity(_skeleton.moveSpeed * _skeleton.faceDirection, rb.velocity.y);
        if (_skeleton.IsTouchingWall()||!_skeleton.IsGrounded()) 
        {
            _skeleton.Flip();
            stateMachine.SwitchState(_skeleton._idleState);
        }
    }
}
