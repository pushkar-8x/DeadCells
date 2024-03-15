using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    [Header("Collisions")]
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] float groundCheckDistance;
    [SerializeField] float wallCheckDistance;
    [SerializeField] LayerMask groundMask;

    public int faceDirection { get; private set; } = 1;
    private bool facingRight = true;



    protected virtual void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public void ZeroVelocity() => rb.velocity = Vector2.zero;

    public bool IsGrounded() => Physics2D.Raycast(groundCheckPoint.position,
        Vector2.down, groundCheckDistance, groundMask);

    public bool IsTouchingWall() => Physics2D.Raycast(wallCheckPoint.position, Vector2.right * faceDirection,
        wallCheckDistance, groundMask);


    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheckPoint.position,
            new Vector2(groundCheckPoint.position.x, groundCheckPoint.position.y - groundCheckDistance));

        Gizmos.DrawLine(wallCheckPoint.position,
            new Vector2(wallCheckPoint.position.x + wallCheckDistance, wallCheckPoint.position.y));
    }

    #region Flip
    public void Flip()
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
    #endregion
}
