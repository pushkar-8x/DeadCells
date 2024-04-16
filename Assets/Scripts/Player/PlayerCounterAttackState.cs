using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounter" , false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();
        Collider2D[] cols = Physics2D.OverlapCircleAll(player.attackPoint.position, player.attackRadius);

        foreach (Collider2D col in cols)
        {
            Enemy enemy = col.GetComponent<Enemy>();

            if (enemy != null && enemy.CanBeStunned())
            {
                stateTimer = 10f;
                player.anim.SetBool("SuccessfulCounter", true);
            }

        }

        if(stateTimer < 0 || triggerCalled)
        {
            stateMachine.SwitchState(player.idleState);
        }
    }
}
