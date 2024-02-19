using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    protected EnemyStateMachine stateMachine;

    protected Enemy enemy;

    private string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;

    public EnemyState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.enemy = enemy;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;

    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {

    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
