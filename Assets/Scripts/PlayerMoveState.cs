using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

   

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.MoveSpeed , rb.velocity.y);

        if (xInput == 0)
            stateMachine.SwitchState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
