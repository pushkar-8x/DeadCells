using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_BattleState : Skeleton_GroundedState
{
    private Transform _playerTransform;
    private int moveDir = 1;
    public Skeleton_BattleState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName,Enemy_Skeleton _skeleton) : base(stateMachine, enemy, animBoolName, _skeleton)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        _playerTransform = GameObject.Find("Player").transform;
        Debug.Log("IM IN BATTLE STATE");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(_skeleton.IsPlayerDetected())
        {
            stateTimer = _skeleton.battleTime;
            if(_skeleton.IsPlayerDetected().distance < _skeleton.attackDistance)
            {
                if(CanAttack())
                {
                    stateMachine.SwitchState(_skeleton._attackState);
                    return;
                }
                              
            }
        }
        else
        {
            if(stateTimer<0||
                Vector2.Distance(_playerTransform.position,_skeleton.transform.position)>_skeleton.maxAgroRange)
            {
                stateMachine.SwitchState(_skeleton._idleState);
            }

        }
        
        if (_playerTransform.position.x > _skeleton.transform.position.x)
        {
            moveDir = 1;
        }
        else if(_playerTransform.position.x < _skeleton.transform.position.x)
        {
            moveDir = -1;
        }
        _skeleton.SetVelocity(_skeleton.moveSpeed * moveDir, rb.velocity.y);

    }

    public bool CanAttack()
    {
        if(Time.time > _skeleton.lastTimeAttacked + _skeleton.attackCooldown)
        {
            return true;
        }
        else { return false; }
    }
}
