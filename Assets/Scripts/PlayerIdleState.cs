using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

    }

   

    public override void Update()
    {
        base.Update();

        if (xInput != 0)
            stateMachine.SwitchState(player.moveState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
