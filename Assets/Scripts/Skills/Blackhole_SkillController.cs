using System.Collections.Generic;
using UnityEngine;

public class Blackhole_SkillController : MonoBehaviour
{
    [SerializeField] Blackhole_HotKey hotKeyPrefab;
    private float maxBlackholeRadius;
    private float growSpeed ;
    private float shrinkSpeed ;
    private bool canGrow = true;
    
    [SerializeField] List<KeyCode> keyCodeList = new List<KeyCode>();

    [Header("Attack")]
    [SerializeField] private float cloneAttackCoolDown;
    [SerializeField] private float cloneOffset = 2f;
    private float cloneAttackTimer;
    private bool canAttack;
    private int amountOfAttacks;
    private bool shouldShrink;


    private List<Transform> targets = new List<Transform>();
    public void AddTargetToList(Transform target) => targets.Add(target);
    private List<Blackhole_HotKey> allHotKeys = new List<Blackhole_HotKey>();

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        

        if (Input.GetKeyDown(KeyCode.X))
        {
            ReleaseCloneAttack();
        }

        if (canAttack && cloneAttackTimer < 0)
        {
            CloneAttackBehaviour();
        }

        if (canGrow && !shouldShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale,
                new Vector2(maxBlackholeRadius, maxBlackholeRadius), growSpeed * Time.deltaTime);

            if (maxBlackholeRadius - transform.localScale.magnitude <= 0.1f)
            {
                /*if (targets.Count <= 0 && !canAttack)
                {
                    shouldShrink = true;                 
                    PlayerManager.instance.player.ExitBlackHole();
                    return;
                }*/
            }
        }

        if(shouldShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale,
                Vector2.zero, shrinkSpeed * Time.deltaTime);

            if (transform.localScale.magnitude <= 0)
                Destroy(gameObject);
        }
    }

    private void CloneAttackBehaviour()
    {        
        cloneAttackTimer = cloneAttackCoolDown;
        int randomIndex = Random.Range(0, targets.Count);

        float rand = Random.Range(0, 100);
        float offset = rand > 50f ? -cloneOffset : cloneOffset;

        SkillManager.instance.cloneSkill.CreateClone(targets[randomIndex], new Vector2(offset, 0));
        amountOfAttacks--;

        if (amountOfAttacks <= 0)
        {
            canAttack = false;
            shouldShrink = true;
            PlayerManager.instance.player.ExitBlackHole();
            
        }
    }

    private void ReleaseCloneAttack()
    {
        canAttack = true;
        canGrow = false;
        DestroyHotKeys();
        PlayerManager.instance.player.SetTransparent(true);
    }

    public void SetupBlackHole(float maxBlackHoleRadius , float growSpeed , float shrinkSpeed , float cloneAttackCoolDown , int amountOfAttacks)
    {
        maxBlackholeRadius = maxBlackHoleRadius;
        this.growSpeed = growSpeed;
        this.shrinkSpeed = shrinkSpeed;
        this.cloneAttackCoolDown = cloneAttackCoolDown;
        this.amountOfAttacks = amountOfAttacks;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.FreezeTime(true);
           // targets.Add(collision.transform);
            CreateHotKey(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.FreezeTime(false);
            targets.Remove(collision.transform);
        }
    }

    private void CreateHotKey(Collider2D collision)
    {
        Blackhole_HotKey newHotKey = Instantiate(hotKeyPrefab, collision.transform.position +
                    new Vector3(0, 2), Quaternion.identity);
        allHotKeys.Add(newHotKey);

        KeyCode randomCode = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(randomCode);

        newHotKey.SetupHotKey(this, randomCode, collision.transform);
    }

    private void DestroyHotKeys()
    {
        if (allHotKeys.Count <= 0) return;
        foreach (var hotKey in allHotKeys)
        {
            Destroy(hotKey.gameObject);
        }
    }
}
