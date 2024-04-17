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

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        canRotate = false;
        rb.isKinematic = true;
        circleCollider.enabled = false;
        anim.SetBool("Rotate", false);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
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
