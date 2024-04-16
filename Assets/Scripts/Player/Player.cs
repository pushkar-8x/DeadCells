using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    #region Public variables

    [Header("Movement")]
    public float MoveSpeed = 8f;
    public float JumpForce = 12f;

    [Header("Dash ")]
    public float dashDuration = 0.2f;
    public float dashSpeed = 25f;
    public float dashDirection { get; private set; }

    [Header("Attack")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;


    #endregion


  
    #region GETTERS


    public bool IsBusy { get; private set; }
    public PlayerStateMachine stateMachine { get; private set; }

    public SkillManager skillManager { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    public PlayerJumpState jumpState { get; private set; }

    public PlayerAirState airState { get; private set; }

    public PlayerDashState dashState { get; private set; }

    public PlayerAttackState attackState { get; private set; }

    public PlayerCounterAttackState counterAttackState { get; private set; }

    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }

    
    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    #endregion


    #region Monobehaviour Methods
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(stateMachine, this, "Idle");
        moveState = new PlayerMoveState(stateMachine, this, "Move");
        jumpState = new PlayerJumpState(stateMachine, this, "Jump");
        airState = new PlayerAirState(stateMachine, this, "Jump");
        dashState = new PlayerDashState(stateMachine, this, "Dash");
        wallSlideState = new PlayerWallSlideState(stateMachine, this, "WallSlide");
        wallJumpState = new PlayerWallJumpState(stateMachine, this, "Jump");
        attackState = new PlayerAttackState(stateMachine, this, "Attack");
        counterAttackState = new PlayerCounterAttackState(stateMachine, this, "CounterAttack");
        skillManager = SkillManager.instance;
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialise(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDash();
    }

    #endregion

    private void CheckForDash()
    {
        if (IsTouchingWall()) return;

        if(Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dashSkill.CanUseSkill())
        {
            dashDirection = Input.GetAxisRaw("Horizontal");

            if (dashDirection == 0)
                dashDirection = faceDirection;

            stateMachine.SwitchState(dashState);
        }
    }

    private IEnumerator SetBusy(float _duration)
    {
        IsBusy = true;
        yield return new WaitForSeconds(_duration);
        IsBusy = false;
    }
   
}
