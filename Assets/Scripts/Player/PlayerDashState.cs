using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skillManager.cloneSkill.CreateClone(player.transform);
        stateTimer = player.dashDuration;
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashSpeed * player.dashDirection , 0f);

        if (player.IsTouchingWall())
        {
            stateMachine.SwitchState(player.wallSlideState);
            return;
        }
            

        if(stateTimer < 0)
        {
            stateMachine.SwitchState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, rb.velocity.y);
    }
}
