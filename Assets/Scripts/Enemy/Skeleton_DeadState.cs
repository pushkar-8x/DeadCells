using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_DeadState : EnemyState
{
    private Enemy_Skeleton _skeleton;
    public Skeleton_DeadState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName, Enemy_Skeleton skeleton) : base(stateMachine, enemy, animBoolName)
    {
        this._skeleton = skeleton;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetBool(enemy.lastAnimBoolName, false);
        enemy.anim.speed = 0;
        _skeleton.capsuleCollider2D.enabled = false;
        stateTimer = 0.2f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
        {
            enemy.SetVelocity(0f , 20f);
        }
    }
}
