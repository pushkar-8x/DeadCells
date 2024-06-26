using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [Header("Move Info")]
    public float moveSpeed = 1.5f;
    public float idleTime = 2f;

    [Header("Attack")]
    public float attackDistance = 2f;
    public float attackCooldown = 2f;
    public float battleTime = 5f;
    public float maxAgroRange = 10f;
    [HideInInspector] public float lastTimeAttacked;
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Stun")]
    public float stunDuration;
    public Vector2 stunDirection;
    public SpriteRenderer counterImage;

    private bool canBeStunned;
    private float defaultMoveSpeed;
    public string lastAnimBoolName { get; private set; }
    public CharacterFX characterFX => GetComponent<CharacterFX>();

    public EnemyStateMachine stateMachine { get; private set; }

    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => 
        Physics2D.Raycast(wallCheckPoint.position, faceDirection * Vector2.right, 50f, whatIsPlayer);

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
       stateMachine.currentState.Update();

        //Debug.Log("Player detected " + IsPlayerDetected());
    }

    public virtual void OpenCounterWindow()
    {
        canBeStunned = true;
        counterImage.gameObject.SetActive(true);
    }

    public virtual void CloseCounterWindow()
    {
        canBeStunned = false;
        counterImage.gameObject.SetActive(false);
    }

    public override void ApplyAilmentToMovement(float _slowPercentage, float _duration)
    {
        moveSpeed = defaultMoveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);
        Invoke(nameof(ResetAilmentEffectsToMovement), _duration);
    }

    public override void ResetAilmentEffectsToMovement()
    {
        base.ResetAilmentEffectsToMovement();
        moveSpeed = defaultMoveSpeed;
    }

    public void FreezeTime(bool shouldFreeze)
    {
        if(shouldFreeze)
        {
            anim.speed = 0f;
            moveSpeed = 0;
        }
        else
        {
            anim.speed = 1f;
            moveSpeed = defaultMoveSpeed ;
        }
    }

    private IEnumerator FreezeTimeForSeconds(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterWindow();
            return true;

        }
        return false;
    }

    public virtual void AssignLastAnimBoolName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance, transform.position.y));
    }
}
