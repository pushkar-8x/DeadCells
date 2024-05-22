using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDownTime = 1f;
    protected float coolDownTimer;
    protected Player player;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        coolDownTimer = coolDownTime;

    }

    protected virtual void Update()
    {
        if(coolDownTimer > -1f) 
        coolDownTimer -= Time.deltaTime;
    }
    public virtual bool CanUseSkill()
    {
        if(coolDownTimer < 0f)
        {
            UseSkill();
            coolDownTimer = coolDownTime;
            return true;
        }

        Debug.Log("Skill on cooldown !");
        return false;
    }

    public virtual void UseSkill()
    {

    }

    public virtual Transform FindClosestEnemy(Transform _refTransform)
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(_refTransform.position, 20f);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider2D col in cols)
        {
            Enemy enemy = col.GetComponent<Enemy>();

            if (enemy != null)
            {
                float currentDistance = Vector2.Distance(_refTransform.position, enemy.transform.position);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestEnemy = enemy.transform;
                }

            }
        }
        return closestEnemy;
    }


}
