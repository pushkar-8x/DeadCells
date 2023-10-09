using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsTouchingWall())
            stateMachine.SwitchState(player.wallSlideState);

        if(player.IsGrounded())
        {
            stateMachine.SwitchState(player.idleState);
        }

        if(xInput != 0f)
        {
            player.SetVelocity(player.MoveSpeed * 0.8f * xInput, rb.velocity.y);
        }
    }
}
