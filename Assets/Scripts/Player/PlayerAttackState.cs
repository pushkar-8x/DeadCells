using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private int comboCounter;

    private float lastAttackedTime;
    private float comboWindow = 2f;
    
    public PlayerAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time >= lastAttackedTime + comboWindow)
            comboCounter = 0;

        xInput = 0;

        Debug.Log(comboCounter);
        player.anim.SetInteger("ComboCounter", comboCounter);

        float attackDir = player.faceDirection;

        if (xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.SwitchState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        comboCounter++;
        lastAttackedTime = Time.time;

        player.StartCoroutine("SetBusy", 0.15f);
    }

  

}
