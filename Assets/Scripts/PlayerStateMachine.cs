using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine 
{
   public PlayerState currentState { get; private set; }

    public void Initialise(PlayerState  state)
    {
        currentState = state;
        currentState.Enter();
    }

    public void SwitchState(PlayerState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
