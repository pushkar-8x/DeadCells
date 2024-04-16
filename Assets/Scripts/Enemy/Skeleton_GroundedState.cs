using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_GroundedState : EnemyState
{
    protected Enemy_Skeleton _skeleton;

    private Transform _playertransform;
    public Skeleton_GroundedState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName , Enemy_Skeleton _skeleton) : base(stateMachine, enemy, animBoolName)
    {
        this._skeleton = _skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        _playertransform = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(_skeleton.IsPlayerDetected() || Vector2.Distance(_playertransform.position , _skeleton.transform.position)<2f)
            stateMachine.SwitchState(_skeleton._battleState);
    }
}
