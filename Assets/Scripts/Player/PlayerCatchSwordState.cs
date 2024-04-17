using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    public PlayerCatchSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        sword = player.playerSword.transform;

        if (player.transform.position.x > sword.position.x
            && player.faceDirection == 1)
        {
            player.Flip();
        }
        else if (player.transform.position.x < sword.position.x
            && player.faceDirection == -1)
        {
            player.Flip();
        }

        rb.velocity = new Vector2(player.swordCatchImpact * player.faceDirection * -1, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("SetBusy", 0.1f);
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
        {
            stateMachine.SwitchState(player.idleState);
        }
    }
}
