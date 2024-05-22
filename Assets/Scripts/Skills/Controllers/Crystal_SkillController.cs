using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_SkillController : MonoBehaviour
{
    private float crystalDuration ;
    private bool canExplode;
    private float explosionRange;
    private bool canMove;
    private float moveSpeed;
    private bool canGrow;
    private float growSpeed;
    private Transform closestEnemy;

    private CircleCollider2D circleCollider2D => GetComponent<CircleCollider2D>();
    private Animator animator => GetComponent<Animator>();


    
    public void SetupCrystal(float crystalDuration, bool canExplode,
        float explosionRange, bool canMove, float moveSpeed, bool canGrow, float growSpeed ,Transform closestEnemy)
    {
        this.crystalDuration = crystalDuration;
        this.canExplode = canExplode;
        this.explosionRange = explosionRange;
        this.canMove = canMove;
        this.moveSpeed = moveSpeed;
        this.canGrow = canGrow;
        this.growSpeed = growSpeed;
        this.closestEnemy = closestEnemy;
    }


    private void Update()
    {
        crystalDuration -= Time.deltaTime;
        if (crystalDuration <= 0)
        {
            FinishCrystalAbility();
        }

        if(canMove && closestEnemy != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, closestEnemy.position) < 0.5f)
            {
                FinishCrystalAbility();
                canMove = false;
            }
        }

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(explosionRange, explosionRange), growSpeed * Time.deltaTime);
    }

    public void FinishCrystalAbility()
    {
        if (canExplode)
        {
            canGrow = true;
            animator.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    public void ExplosionAnimationEvent()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, circleCollider2D.radius);
        foreach (Collider2D col in cols)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            enemy?.Damage();
        }
    }

    public void SelfDestroy() => Destroy(gameObject);
}
