using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine playerStateMachine, Player player, string animBoolName) : base(playerStateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

   

    public override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {
            playerStateMachine.SwitchState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
