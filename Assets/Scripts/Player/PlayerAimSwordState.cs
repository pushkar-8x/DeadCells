using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.rb.velocity = Vector2.zero;
        SkillManager.instance.swordSkill.ActivateDots(true);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("SetBusy", 0.2f);
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            stateMachine.SwitchState(player.idleState);
        }
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (player.transform.position.x > mousePosition.x 
            && player.faceDirection == 1)
        {
            player.Flip();
        }           
        else if (player.transform.position.x < mousePosition.x 
            && player.faceDirection == -1)
        {
            player.Flip();
        }
            
    }
}
