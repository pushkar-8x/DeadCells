using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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

        if(Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
        {
            stateMachine.SwitchState(player.aimSwordState);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.SwitchState(player.counterAttackState);
        }

        if (!player.IsGrounded())
            stateMachine.SwitchState(player.airState);

        if(Input.GetKeyDown(KeyCode.Space) && player.IsGrounded())
            stateMachine.SwitchState(player.jumpState);

        if (Input.GetMouseButtonDown(0))
            stateMachine.SwitchState(player.attackState);
    }

    private bool HasNoSword()
    {
        if(!player.playerSword )
        {
            return true;
        }

        player.playerSword.GetComponent<SwordSkill_Controller>().ReturnSword();
        return false;
    }
}
