using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill_Controller : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D circleCollider;
    private Rigidbody2D rb;
    private Player player;
    private bool canRotate = true;
    private bool isReturning;
    [SerializeField] float returnSpeed = 12f;

    [Header("Bounce Info")]
    [SerializeField] private bool canBounce = true;
    [SerializeField] private int bounceCount = 4;
    [SerializeField] private float bounceSpeed = 20f;

    private List<Transform> enemyTargets  = new List<Transform>();

    private int targetIndex;



    private void Awake()
    {
        anim= GetComponentInChildren<Animator>();
        circleCollider= GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetupSword(Vector2 _dir , float gravityScale , Player player)
    {
        this.player = player;
        rb.velocity = _dir;
        rb.gravityScale = gravityScale;
        anim.SetBool("Rotate", true);
    }

    private void Update()
    {
        if(canRotate)
        transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position , returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1.0f)
                player.CatchSword();
        }

        if( canBounce && enemyTargets.Count > 0 )
        {
            transform.position =  Vector2.MoveTowards(transform.position , enemyTargets[targetIndex].position , bounceSpeed * Time.deltaTime);
            float dist = Vector2.Distance(transform.position, enemyTargets[targetIndex].position);
            if(dist < 0.1f)
            {
                targetIndex++;
                bounceCount--;

                if(bounceCount <= 0 )
                {
                    canBounce = false;
                    isReturning = true;
                }
                
                if(targetIndex >= enemyTargets.Count)
                    targetIndex = 0;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        Enemy enemy = collision.GetComponent<Enemy>();

        if(enemy != null && canBounce && enemyTargets.Count <= 0)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 10f);

            foreach (Collider2D col in enemies)
            {
                Enemy targetEnemy = col.GetComponent<Enemy>();
                if (targetEnemy != null)
                {
                    enemyTargets.Add(targetEnemy.transform);
                }                   

            }
        }

        SetupHitBehaviour(collision);
    }

    private void SetupHitBehaviour(Collider2D collision)
    {
        canRotate = false;
        rb.isKinematic = true;
        circleCollider.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        

        if(canBounce && enemyTargets.Count>0) 
        {
            return;
        }

        anim.SetBool("Rotate", false);
        transform.parent = collision.transform;
    }

    public void ReturnSword()
    {
        //rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isReturning = true;
        transform.parent = null;
    }
}
