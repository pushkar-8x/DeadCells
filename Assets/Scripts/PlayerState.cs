using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine playerStateMachine;

    protected Player player;

    private string animBoolName;


    public PlayerState(PlayerStateMachine playerStateMachine, Player player, string animBoolName)
    {
        this.playerStateMachine = playerStateMachine;
        this.player = player;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        Debug.Log("I enter "+ animBoolName);
        player.anim.SetBool(animBoolName , true);

    }

    public virtual void Update()
    {
        Debug.Log("I update " + animBoolName);
    }

    public virtual void Exit()
    {
        Debug.Log("I exit " + animBoolName);
        player.anim.SetBool(animBoolName , false);

    }
}
