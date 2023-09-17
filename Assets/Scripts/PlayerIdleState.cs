using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine, Player player, string animBoolName) : base(playerStateMachine, player, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

    }

   

    public override void Update()
    {
        base.Update();

        if(Input.GetMouseButtonDown(0))
        {
            playerStateMachine.SwitchState(player.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
