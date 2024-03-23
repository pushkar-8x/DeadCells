using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;

    protected Enemy enemy;
    protected Rigidbody2D rb;
    private string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;

    

    public EnemyState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName )
    {
        this.stateMachine = stateMachine;
        this.enemy = enemy;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemy.anim.SetBool(animBoolName, true);
        this.rb = enemy.rb;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemy.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
