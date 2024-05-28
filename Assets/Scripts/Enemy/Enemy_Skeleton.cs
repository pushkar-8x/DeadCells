using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    public Skeleton_IdleState _idleState { get; private set; }
    public Skeleton_MoveState _moveState { get; private set; }

    public Skeleton_BattleState _battleState { get; private set; }

    public Skeleton_AttackState _attackState { get; private set; }

    public Skeleton_StunnedState _stunState { get; private set; }

    public Skeleton_DeadState _deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _idleState = new Skeleton_IdleState(stateMachine, this, "Idle", this);
        _moveState = new Skeleton_MoveState(stateMachine, this, "Move", this);
        _battleState = new Skeleton_BattleState(stateMachine, this, "Move", this);
        _attackState = new Skeleton_AttackState(stateMachine, this, "Attack", this);
        _stunState = new Skeleton_StunnedState(stateMachine, this, "Stunned", this);
        _deadState = new Skeleton_DeadState(stateMachine, this, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialise(_idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned()
    {
       if(base.CanBeStunned())
        {
            stateMachine.SwitchState(_stunState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.SwitchState(_deadState);
    }
}
