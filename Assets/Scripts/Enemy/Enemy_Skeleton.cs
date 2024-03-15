using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    public Skeleton_IdleState _idleState { get; private set; }
    public Skeleton_IdleState _moveState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _idleState = new Skeleton_IdleState(stateMachine, this, "Idle", this);
        _moveState = new Skeleton_IdleState(stateMachine, this, "Move", this);
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
