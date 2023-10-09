using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    public Player player => GetComponentInParent<Player>();

    public void OnAnimationFinished()
    {
        player.AnimationFinishTrigger();
    }
}
