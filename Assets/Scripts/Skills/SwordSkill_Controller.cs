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
    private float returnSpeed;

    [Header("Bounce Info")]
    private bool canBounce ;
    private int bounceCount ;
    private float bounceSpeed ;

    [Header("Pierce Info")]
    private int pierceCount;

    [Header("Spin Info")]
    [SerializeField] private float spinHitSpeed = 1.5f;

    [Header("Destroy Info")]
    [SerializeField] private float destroyTime = 5f;
    [SerializeField] private float blinkDuration = 2.0f;
    [SerializeField] private float blinkInterval = 0.1f;


    private float spinDuration;
    private float spinTimer;
    private bool isSpinning;
    private float maxSpinDistance;
    private bool wasStopped;
    private float hitTimer;
    private float hitCoolDown;
    private float spinHitDirection;
    private float freezeDuration; 
    private List<Transform> enemyTargets  = new List<Transform>();
    private int targetIndex;
    private SpriteRenderer swordSpriteRenderer;

    private void Awake()
    {
        anim= GetComponentInChildren<Animator>();
        swordSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        circleCollider= GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        Invoke(nameof(DestroyMe), destroyTime);
    }

    

    public void SetupSword(Vector2 _dir , float gravityScale , Player player , float freezeDuration , float returnSpeed)
    {
        this.player = player;
        rb.velocity = _dir;
        rb.gravityScale = gravityScale;
        this.freezeDuration = freezeDuration;
        this.returnSpeed = returnSpeed;

        if(pierceCount <= 0)
        anim.SetBool("Rotate", true);

        spinHitDirection = Mathf.Clamp(rb.velocity.x, -1f, 1f);
    }

    public void SetupBounce(bool canBounce, int bounceCount , float bounceSpeed)
    {
        this.canBounce = canBounce;
        this.bounceCount = bounceCount;
        this.bounceSpeed = bounceSpeed;
    }

    public void SetupPierce(int pierceCount)
    {
        this.pierceCount = pierceCount;
    }

    public void SetupSpin(bool canSpin , float spinDuration , float maxSpinDistance , float hitCoolDown)
    {
        isSpinning = canSpin;
        this.spinDuration = spinDuration;
        this.maxSpinDistance = maxSpinDistance;
        this.hitCoolDown = hitCoolDown;
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1.0f)
                player.CatchSword();
        }
        BounceSwordBehaviour();
        SpinSwordBehaviour();
    }

    private void SpinSwordBehaviour()
    {
        if (isSpinning)
        {
            float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distToPlayer > maxSpinDistance && !wasStopped)
            {
                StopWhileSpinning();
            }

            if (wasStopped)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                        new Vector2(transform.position.x + spinHitDirection, transform.position.y), spinHitSpeed * Time.deltaTime);

                spinTimer -= Time.deltaTime;
                if (spinTimer <= 0)
                {
                    isSpinning = false;
                    isReturning = true;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCoolDown;

                    

                    Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 1f);
                    foreach (Collider2D col in enemies)
                    {
                        Enemy targetEnemy = col.GetComponent<Enemy>();
                        ApplyDamage(targetEnemy);
                    }
                }
            }
        }
    }

    private void StopWhileSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        spinTimer = spinDuration;
    }

    private void BounceSwordBehaviour()
    {
        if (canBounce && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);
            float dist = Vector2.Distance(transform.position, enemyTargets[targetIndex].position);
            if (dist < 0.1f)
            {
                Enemy enemy = enemyTargets[targetIndex].GetComponent<Enemy>();
                ApplyDamage(enemy);
                targetIndex++;
                bounceCount--;
                if (bounceCount <= 0)
                {
                    canBounce = false;
                    isReturning = true;
                }
                if (targetIndex >= enemyTargets.Count)
                    targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        Enemy enemy = collision.GetComponent<Enemy>();
        ApplyDamage(enemy);
        SetupEnemyTargets(enemy);
        SetupHitBehaviour(collision);
    }

    private void ApplyDamage(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy?.Damage();
            enemy.StartCoroutine("FreezeTimeForSeconds", freezeDuration);
        }
    }

    private void SetupEnemyTargets(Enemy enemy)
    {
        if (enemy != null && canBounce && enemyTargets.Count <= 0)
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
    }

    private void SetupHitBehaviour(Collider2D collision)
    {
        if (pierceCount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceCount--;
            return;
        }

        if(isSpinning)
        {
            StopWhileSpinning();
            return;
        }

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

    private void DestroyMe()
    {
        StartCoroutine(BlinkAndDestroyCoroutine(swordSpriteRenderer));
    }

    private IEnumerator BlinkAndDestroyCoroutine(SpriteRenderer spriteRenderer)
    {
        // Blink for a certain duration
        float timer = 0f;
        Color originalColor = spriteRenderer.color;
        while (timer < blinkDuration)
        {
            Color newColor = originalColor;
            newColor.a = Mathf.PingPong(Time.time * 5f, 1.0f); // Blinking effect
            spriteRenderer.color = newColor;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // Reset the alpha to 1.0f before destroying
        Color resetColor = originalColor;
        resetColor.a = 1.0f;
        spriteRenderer.color = resetColor;

        // Destroy the GameObject
        Destroy(gameObject);
    }
}
