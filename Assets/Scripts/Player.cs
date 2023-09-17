using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Animator anim { get; private set; }

    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }


    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(stateMachine, this, "Idle");
        moveState = new PlayerMoveState(stateMachine, this, "Move");
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        stateMachine.Initialise(idleState);

        
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }
}
