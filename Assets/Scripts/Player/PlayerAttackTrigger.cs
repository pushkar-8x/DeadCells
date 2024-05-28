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

    public void AttackTrigger()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(player.attackPoint.position, player.attackRadius);

        foreach (Collider2D col in cols)
        {
            
            EnemyStats enemyStats= col.GetComponent<EnemyStats>();
            //enemy?.Damage();
            //enemy?.characterStats.TakeDamage(player.characterStats.damage.GetValue());
            if(enemyStats != null)
            player.characterStats.ApplyDamage(enemyStats);
        }
    }

    public void ThrowSword()
    {
        SkillManager.instance.swordSkill.CreateSword();
    }
}
