using UnityEngine;

public class Thunder_Controls : MonoBehaviour
{
    private CharacterStats targetStats;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] Vector2 offset;
    private Animator anim;
    private bool hasCompleted = false;
    private int damage;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void SetupBoltAttributes(CharacterStats targetStats , int damage)
    {
        this.targetStats = targetStats;
        this.damage = damage;
    }
    private void Update()
    {
        if(hasCompleted) return;

        transform.position =  Vector2.MoveTowards(transform.position , targetStats.transform.position , moveSpeed * Time.deltaTime);
        transform.right =  transform.position - targetStats.transform.position;

        float distToTarget = Vector2.Distance(transform.position, targetStats.transform.position);
        if (distToTarget < 0.1f)
        {
            anim.transform.position -= (Vector3)offset;
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector2(3, 3);
            anim.transform.rotation = Quaternion.identity;
            anim.SetTrigger("Hit");
            hasCompleted = true;
            Invoke(nameof(DestroySelf), 0.2f);
        }
    }

    private void DestroySelf()
    {
        targetStats.TakeDamage(2);
        Destroy(gameObject,0.4f);
    }

}
