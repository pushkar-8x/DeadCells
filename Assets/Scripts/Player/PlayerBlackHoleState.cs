using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{

    private float flyDuration = 0.4f;
    private float flySpeed = 15f;
    private float fallSpeed = 0.1f;
    private bool skillUsed;
    private float defaultGravityScale;
    public PlayerBlackHoleState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        skillUsed = false;
        defaultGravityScale = rb.gravityScale;
        rb.gravityScale = 0f;
        stateTimer = flyDuration;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravityScale;
        PlayerManager.instance.player.SetTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer > 0f )
        {
            rb.velocity = new Vector2(0, flySpeed);
            
        }
        else
        {
            rb.velocity = new Vector2(0, -fallSpeed);
            if (!skillUsed)
            {
                if (player.skillManager.blackHoleSkill.CanUseSkill())
                    skillUsed = true;
            }
        }
    }
}
