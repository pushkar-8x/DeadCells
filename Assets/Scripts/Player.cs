using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed = 8f;
    public float JumpForce = 12f;

    [Header("Dash ")]
    public float dashDuration = 0.2f;
    public float dashSpeed = 25f;
    public float dashCoolDown = 1f;

    private float dashUsageTimer;
    public float dashDirection { get; private set; }

    [Header("Collisions")]
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] float groundCheckDistance;
    [SerializeField] float wallCheckDistance;
    [SerializeField] LayerMask groundMask;

    private bool facingRight = true;
    public int faceDirection { get; private set; } = 1;

    public Animator anim { get; private set; }

    public PlayerStateMachine stateMachine { get; private set; }

    public Rigidbody2D rb { get; private set;}

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    public PlayerJumpState jumpState { get; private set; }

    public PlayerAirState airState { get; private set; }

    public PlayerDashState dashState { get; private set; }

    public PlayerWallSlideState wallSlideState { get; private set; }


    public bool IsGrounded() => Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckDistance, groundMask);

    public bool IsTouchingWall() => Physics2D.Raycast(wallCheckPoint.position, Vector2.right * faceDirection, wallCheckDistance, groundMask);
    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        rb = GetComponent<Rigidbody2D>();
        idleState = new PlayerIdleState(stateMachine, this, "Idle");
        moveState = new PlayerMoveState(stateMachine, this, "Move");
        jumpState = new PlayerJumpState(stateMachine, this, "Jump");
        airState = new PlayerAirState(stateMachine, this, "Jump");
        dashState = new PlayerDashState(stateMachine, this, "Dash");
        wallSlideState = new PlayerWallSlideState(stateMachine, this, "WallSlide");
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        stateMachine.Initialise(idleState);

        
    }

    private void Update()
    {
        stateMachine.currentState.Update();
        CheckForDash();
    }

    private void CheckForDash()
    {
        dashUsageTimer -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0f)
        {
            dashUsageTimer = dashCoolDown;
            dashDirection = Input.GetAxisRaw("Horizontal");

            if (dashDirection == 0)
                dashDirection = faceDirection;

            stateMachine.SwitchState(dashState);
        }
    }

    public void SetVelocity(float xVelocity , float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheckPoint.position ,
            new Vector2(groundCheckPoint.position.x, groundCheckPoint.position.y - groundCheckDistance));

        Gizmos.DrawLine(wallCheckPoint.position,
            new Vector2(wallCheckPoint.position.x + wallCheckDistance, wallCheckPoint.position.y));
    }

    private void Flip()
    {
        faceDirection = -1 * faceDirection;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController(float _xVel)
    {
        if (_xVel > 0 && !facingRight)
            Flip();
        else if (_xVel < 0 && facingRight)
            Flip();
    }
}
