using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_AnimationTrigger : MonoBehaviour
{
    public Enemy_Skeleton _skeleton => GetComponentInParent<Enemy_Skeleton>();

    public void OnAnimationFinished()
    {
        _skeleton.AnimationFinishTrigger();
    }

    public void AttackTrigger()
    {

        Collider2D[] cols = Physics2D.OverlapCircleAll(_skeleton.attackPoint.position, _skeleton.attackRadius);

        foreach (Collider2D col in cols)
        {
            PlayerStats playerStats = col.GetComponent<PlayerStats>();
            //player?.DamageEffects();
            if (playerStats != null)
                _skeleton.characterStats.ApplyDamage(playerStats);
        }
    }

    private void OpenCounterAttackWindow() => _skeleton.OpenCounterWindow();
    private void CloseCounterAttackWindow() => _skeleton.CloseCounterWindow();
}
