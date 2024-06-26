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
    private CircleCollider2D circleCollider2D;
    private float blackHoleTimer;
    private bool playerCanDisappear = true;

    public List<Transform> targets = new List<Transform>();

    public bool playerCanExitState { get; private set; }
    public void AddTargetToList(Transform target) => targets.Add(target);
    private List<Blackhole_HotKey> allHotKeys = new List<Blackhole_HotKey>();

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.enabled = true;
    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;

        if(blackHoleTimer < 0f)
        {
            blackHoleTimer = Mathf.Infinity;
            if(targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackHole();
            }           
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            ReleaseCloneAttack();
        }

        if (canAttack && cloneAttackTimer < 0 && amountOfAttacks > 0)
        {
            CloneAttackBehaviour();
        }

        if (canGrow && !shouldShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale,
                new Vector2(maxBlackholeRadius, maxBlackholeRadius), growSpeed * Time.deltaTime);
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

        if(SkillManager.instance.cloneSkill.createCrystalInsteadOfClone)
        {
            SkillManager.instance.crystalSkill.CreateCrystal();
            SkillManager.instance.crystalSkill.CrystalChooseRandomTarget();
        }
        else
        {
            SkillManager.instance.cloneSkill.CreateClone(targets[randomIndex], new Vector2(offset, 0));
        }  
        
        amountOfAttacks--;

        if (amountOfAttacks <= 0)
        {
            FinishBlackHole();
        }
    }

    private void FinishBlackHole()
    {
        DestroyHotKeys();
        playerCanExitState = true;
        canAttack = false;
        shouldShrink = true;
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0) return;
        canAttack = true;
        canGrow = false;
        DestroyHotKeys();
        if(playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.instance.player._characterFx.SetTransparent(true);
        }
        
    }

    public void SetupBlackHole(float maxBlackHoleRadius , float growSpeed , float shrinkSpeed , float cloneAttackCoolDown , int amountOfAttacks , float blackHoleDuration)
    {
        this.maxBlackholeRadius = maxBlackHoleRadius;
        blackHoleTimer = blackHoleDuration;
        this.growSpeed = growSpeed;
        this.shrinkSpeed = shrinkSpeed;
        this.cloneAttackCoolDown = cloneAttackCoolDown;
        this.amountOfAttacks = amountOfAttacks;

        if(SkillManager.instance.cloneSkill.createCrystalInsteadOfClone)
            playerCanDisappear = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.FreezeTime(true);
            //targets.Add(collision.transform);
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
        allHotKeys.Clear();
    }
}
