using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill_Controller : MonoBehaviour
{

    [SerializeField] private float colorLossSpeed = 1f;
    private float cloneTimer;
    private SpriteRenderer sr;

    [Space]
    public Transform cloneAttackPoint;
    public float cloneAttackRadius = 0.8f;

    private Animator animator;
    private Transform closestEnemy;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0f)
        {
            sr.color = new Color(1, 1, 1, (sr.color.a - Time.deltaTime * colorLossSpeed));

            if(sr.color.a <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetupClone(Transform newTransform , float cloneDuration , bool _canAttack , Vector2 offset , Transform _closestEnemy)
    {
        if(_canAttack)
        {
            animator.SetInteger("AttackCounter", Random.Range(1, 4));
        }

        transform.position = newTransform.position + (Vector3)offset;
        cloneTimer = cloneDuration;
        closestEnemy = _closestEnemy;
        FaceEnemy();
    }

    public void OnAnimationFinished()
    {
        cloneTimer = -1f;
    }

    public void AttackTrigger()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(cloneAttackPoint.position, cloneAttackRadius);

        foreach (Collider2D col in cols)
        {

            Enemy enemy = col.GetComponent<Enemy>();
            enemy?.Damage();
        }
    }

    private void FaceEnemy()
    {
        if(closestEnemy != null)
        {
            if ( transform.position.x > closestEnemy.transform.position.x )
                transform.Rotate(0, 180, 0);
        }
    }
}
