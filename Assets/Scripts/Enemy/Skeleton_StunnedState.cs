using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_StunnedState : EnemyState
{
    private Enemy_Skeleton _skeleton;
    public Skeleton_StunnedState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName, Enemy_Skeleton skeleton) : base(stateMachine, enemy, animBoolName)
    {
        _skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = _skeleton.stunDuration;
        rb.velocity = new Vector2(-_skeleton.faceDirection * _skeleton.stunDirection.x,
            _skeleton.stunDirection.y);

        _skeleton.characterFX.InvokeRepeating("BlinkSprite", 0, 0.1f);
    }

    public override void Exit()
    {
        base.Exit();
        _skeleton.characterFX.Invoke("CancelBlink",0f);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.SwitchState(_skeleton._idleState);
        
    }
}
