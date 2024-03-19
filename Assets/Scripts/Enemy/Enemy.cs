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

    [SerializeField] protected LayerMask whatIsPlayer;

    public EnemyStateMachine stateMachine { get; private set; }

    public virtual RaycastHit2D IsPlayerDetected() => 
        Physics2D.Raycast(wallCheckPoint.position, faceDirection * Vector2.right, 50f, whatIsPlayer);

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();
       stateMachine.currentState.Update();

        //Debug.Log("Player detected " + IsPlayerDetected());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance, transform.position.y));
    }
}
