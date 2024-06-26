using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;

    protected Player player;

    protected Rigidbody2D rb;

    private string animBoolName ;

    protected float xInput;
    protected float yInput;
    protected float stateTimer;
    protected bool triggerCalled;
    protected bool pauseStateTimer;

    public PlayerState(PlayerStateMachine stateMachine, Player player, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.player = player;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        this.rb = player.rb;
        triggerCalled = false;
        player.anim.SetBool(animBoolName , true);

    }

    public virtual void Update()
    {
        if(!pauseStateTimer)
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        player.anim.SetFloat("yVelocity" , rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName , false);
        
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
