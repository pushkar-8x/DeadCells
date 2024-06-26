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
    public float swordCatchImpact = 7f;


    private float defaultMoveSpeed;
    private float defaultJumpForce;
    private float defaultDashSpeed;

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

    public GameObject playerSword { get; private set; }

    public PlayerCounterAttackState counterAttackState { get; private set; }

    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }

    public PlayerBlackHoleState blackHoleState { get; private set; }

    public PlayerDeadState playerDeadState { get; private set; }


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
        aimSwordState = new PlayerAimSwordState(stateMachine,this, "AimSword");
        catchSwordState = new PlayerCatchSwordState(stateMachine, this, "CatchSword");
        blackHoleState = new PlayerBlackHoleState(stateMachine, this, "Jump");
        playerDeadState = new PlayerDeadState(stateMachine, this, "DieMF");

    }

    protected override void Start()
    {
        base.Start();
        skillManager = SkillManager.instance;
        stateMachine.Initialise(idleState);

        defaultMoveSpeed = MoveSpeed;
        defaultJumpForce = JumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDash();

        if (Input.GetKeyDown(KeyCode.C))
        {
            skillManager.crystalSkill.CanUseSkill();
        }
    }

    #endregion

    public override void ApplyAilmentToMovement(float _slowPercentage, float _duration)
    {
        MoveSpeed = defaultMoveSpeed * (1 - _slowPercentage);
        JumpForce = defaultJumpForce * (1 - _slowPercentage);
        dashSpeed = defaultDashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);
        Invoke(nameof(ResetAilmentEffectsToMovement), _duration);

    }

    public override void ResetAilmentEffectsToMovement()
    {
        base.ResetAilmentEffectsToMovement();
        MoveSpeed = defaultMoveSpeed;
        JumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public void AssignSword(GameObject sword)
    {
        playerSword = sword;
    }

    public void CatchSword()
    {
        stateMachine.SwitchState(catchSwordState);
        Destroy(playerSword);
    }

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

    public override void Die()
    {
        base.Die();
        stateMachine.SwitchState(playerDeadState);
    }

}
