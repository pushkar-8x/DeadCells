using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;

    protected Player player;

    protected Rigidbody2D rb;

    private string animBoolName;

    protected float xInput;
    protected float yInput;
    protected float stateTimer;

    public PlayerState(PlayerStateMachine stateMachine, Player player, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.player = player;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        this.rb = player.rb;
        player.anim.SetBool(animBoolName , true);

    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        Debug.Log("Horizontal :" + xInput);
        Debug.Log("Vertical :" + yInput);

        player.anim.SetFloat("yVelocity" , rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName , false);

    }
}
