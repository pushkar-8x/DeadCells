using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    public Skeleton_IdleState _idleState { get; private set; }
    public Skeleton_MoveState _moveState { get; private set; }

    public Skeleton_BattleState _battleState { get; private set; }

    public Skeleton_AttackState _attackState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _idleState = new Skeleton_IdleState(stateMachine, this, "Idle", this);
        _moveState = new Skeleton_MoveState(stateMachine, this, "Move", this);
        _battleState = new Skeleton_BattleState(stateMachine, this, "Move", this);
        _attackState = new Skeleton_AttackState(stateMachine, this, "Attack", this);
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
}
