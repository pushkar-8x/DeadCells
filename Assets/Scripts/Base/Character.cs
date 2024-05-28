using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    public CapsuleCollider2D capsuleCollider2D { get; private set; }
    public CharacterStats characterStats { get; private set; }

    [Header("Collisions")]
    [SerializeField]protected Transform groundCheckPoint;
    [SerializeField]protected Transform wallCheckPoint;
    [SerializeField]protected float groundCheckDistance;
    [SerializeField]protected float wallCheckDistance;
    [SerializeField]protected LayerMask groundMask;

    [Header("Attack")]
    public Transform attackPoint;
    public float attackRadius;
    public Vector2 knockBackDirection = new Vector2(7,12);
    public float knockBackDuration = 0.5f;
    

    public int faceDirection { get; private set; } = 1;
    private bool facingRight = true;
    private bool isKnocked;
    private SpriteRenderer sr;

    private CharacterFX _characterFx;

    protected virtual void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _characterFx = GetComponent<CharacterFX>();
        characterStats = GetComponent<CharacterStats>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public void SetZeroVelocity()
    {
        if (isKnocked) return;
        rb.velocity = Vector2.zero;
    }

    public bool IsGrounded() => Physics2D.Raycast(groundCheckPoint.position,
        Vector2.down, groundCheckDistance, groundMask);

    public bool IsTouchingWall() => Physics2D.Raycast(wallCheckPoint.position, Vector2.right * faceDirection,
        wallCheckDistance, groundMask);


    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked) return;
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public virtual void DamageEffects()
    {
        _characterFx.PlayFlash();
        StartCoroutine("KnockBackRoutine");
        Debug.Log("Damaged:" + gameObject.name);

    }

    public virtual void Die()
    {
        
    }

    public void SetTransparent(bool _transparent)
    {
        sr.color = _transparent ? Color.clear : Color.white;
    }

    private IEnumerator KnockBackRoutine()
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockBackDirection.x * -faceDirection, knockBackDirection.y);
        yield return new WaitForSeconds(knockBackDuration);

        isKnocked = false;
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheckPoint.position,
            new Vector2(groundCheckPoint.position.x, groundCheckPoint.position.y - groundCheckDistance));

        Gizmos.DrawLine(wallCheckPoint.position,
            new Vector2(wallCheckPoint.position.x + wallCheckDistance, wallCheckPoint.position.y));

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
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
