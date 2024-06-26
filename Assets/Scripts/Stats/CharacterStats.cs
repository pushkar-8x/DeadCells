using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{

    [Header("Base Stats")]
    public Stat strength;//damage+1 , critpower +1
    public Stat agility;// evasion +1% , critRate 1%
    public Stat intelligence;// magic damage +1 , magic resistance +1
    public Stat vitality;

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critRate;
    public Stat critDamage;

    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamageStat;
    public Stat iceDamageStat;
    public Stat lightningDamageStat;

    private bool isIgnited;//does damage over time
    private bool isChilled;//reduces armor by 20%
    private bool isShocked;//increases evasion by 20%

    [SerializeField] float ailmentStateDuration = 5f;
    [SerializeField] float ignitionDamageCoolDown = 1f;


    [SerializeField] GameObject lightningBoltPrefab;
    private float ignitionStateTimer;
    private float ignitionDamageTimer;

    private int ignitionDamage;
    private int lightningDamage;

    private float chilledStateTimer;
    private float shockedStateTimer;
    private CharacterFX _characterFx;
    public int currentHealth;
    public Action OnHealthChanged;
    public int GetMaxHealthWithModifiers() => maxHealth.GetValue() + vitality.GetValue() * 5;
    private Character _character;
    protected bool isDead;

    protected virtual void Awake()
    {
        _characterFx = GetComponent<CharacterFX>();
        _character = GetComponent<Character>();
    }

    protected virtual void Start()
    {
        critDamage.SetBaseValue(150);
        currentHealth = GetMaxHealthWithModifiers();
    }

    private void Update()
    {
        ignitionStateTimer -= Time.deltaTime;
        chilledStateTimer -= Time.deltaTime;
        shockedStateTimer -= Time.deltaTime;
        ignitionDamageTimer -= Time.deltaTime;


        if (chilledStateTimer <= 0)
        {
            isChilled = false;
        }

        if (shockedStateTimer <= 0)
        {
            isShocked = false;
        }

        if (ignitionStateTimer <= 0)
        {
            isIgnited = false;
        }

        if(isIgnited)
        ApplyIgniteDamage();
    }

    private void ApplyIgniteDamage()
    {
        if (ignitionDamageTimer <= 0)
        {
            UpdateHealthValue(ignitionDamage);
            if (currentHealth <= 0 && !isDead)
            {
                Die();
            }
            ignitionDamageTimer = ignitionDamageCoolDown;
        }
    }

    public virtual void ApplyDamage(CharacterStats targetStats)
    {
        CanTargetAvoidAttack(targetStats);
        int totalDamage = strength.GetValue() + damage.GetValue();
        if (CanCrit())
        {

            totalDamage = CalculateCritDamage(totalDamage);
            Debug.Log("Critical Hit..." + totalDamage);
        }



        totalDamage = FilterDamageWithArmor(targetStats, totalDamage);
        targetStats.TakeDamage(totalDamage);
        //ApplyMagicDamage(targetStats);


    }


    private int FilterDamageWithArmor(CharacterStats targetStats, int totalDamage)
    {
        int currentArmorValue = isChilled ? (int)(targetStats.armor.GetValue() * 0.8f) : targetStats.armor.GetValue();

        totalDamage -= currentArmorValue;
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool CanTargetAvoidAttack(CharacterStats targetStats)
    {
        int totalEvasion = targetStats.evasion.GetValue() + targetStats.agility.GetValue();
        if (isShocked)
            totalEvasion += 20; ;

        
        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Missed!");
            return true;
        }
        return false;
    }

    public virtual void TakeDamage(int _damage)
    {
        UpdateHealthValue(_damage);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());

        _character.DamageEffects();
        _characterFx.PlayFlash();

        Debug.Log(gameObject.name + " takes " + _damage + " damage and has " + currentHealth + " health left.");
        if (currentHealth == 0 && !isDead)
        {
            Die();
        }
    }

    private void UpdateHealthValue(int _damage)
    {
        currentHealth -= _damage;
        OnHealthChanged?.Invoke();
    }

    private bool CanCrit()
    {
        int totalCritRate = agility.GetValue() + critRate.GetValue();
        if (Random.Range(0, 100) < totalCritRate)
        {
            return true;
        }
        return false;
    }

    public void ApplyMagicDamage(CharacterStats targetStats)
    {
        int fireDamageValue = fireDamageStat.GetValue();
        int iceDamageValue = iceDamageStat.GetValue();
        int lightningDamageValue = lightningDamageStat.GetValue();

        int totalMagicDamage = fireDamageStat.GetValue() + iceDamageStat.GetValue() + lightningDamageStat.GetValue() + intelligence.GetValue();
        totalMagicDamage = ApplyTargetResistance(targetStats, totalMagicDamage);
        targetStats.TakeDamage(totalMagicDamage);


        if (Mathf.Max(fireDamageValue, iceDamageValue, lightningDamageValue) <= 0)
        {
            return;
        }

        TryApplyAilments(targetStats, fireDamageValue, iceDamageValue, lightningDamageValue);

    }

    private void TryApplyAilments(CharacterStats targetStats, int fireDamageValue, int iceDamageValue, int lightningDamageValue)
    {
        bool canApplyIgnite = fireDamageValue > iceDamageValue && fireDamageValue > lightningDamageValue;
        bool canApplyChill = iceDamageValue > fireDamageValue && iceDamageValue > lightningDamageValue;
        bool canApplyShock = lightningDamageValue > fireDamageValue && lightningDamageValue > iceDamageValue;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < 0.2f && fireDamageValue > 0)
            {
                canApplyIgnite = true;
                targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Applied fire ..");
                return;
            }
            if (Random.value < 0.4f && iceDamageValue > 0)
            {
                canApplyChill = true;
                targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Applied ice ..");
                return;
            }
            if (Random.value < 0.6f && lightningDamageValue > 0)
            {
                canApplyShock = true;
                targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Applied lightning ..");
                return;
            }
        }


        if (canApplyIgnite)
        {
            targetStats.SetupIgniteDamage(Mathf.RoundToInt(fireDamageValue * 0.2f));
        }

        if (canApplyShock)
        {
            targetStats.SetupIgniteDamage(Mathf.RoundToInt(lightningDamageValue * 0.1f));
        }

        targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private static int ApplyTargetResistance(CharacterStats targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= targetStats.magicResistance.GetValue() + targetStats.intelligence.GetValue();
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    private void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
       /* if(this.isIgnited || this.isChilled || this.isShocked)
        {
            return;
        }
*/
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyLightning = !isIgnited && !isChilled;

        if(_ignite && canApplyIgnite)
        {
            ignitionStateTimer = ailmentStateDuration;
            this.isIgnited = _ignite;
            _characterFx.PlayAilmentEffects(AilmentType.Ignited , ailmentStateDuration);
        }
        if(_chill && canApplyChill)
        {
            this.isChilled = _chill;
            chilledStateTimer = ailmentStateDuration;
            _characterFx.PlayAilmentEffects(AilmentType.Chilled , ailmentStateDuration);
            GetComponent<Character>().ApplyAilmentToMovement(0.2f, ailmentStateDuration);

        }
        if(_shock && canApplyLightning)
        {
            if(!this.isShocked)
            {
                this.isShocked = _shock;
                shockedStateTimer = ailmentStateDuration;
                _characterFx.PlayAilmentEffects(AilmentType.Electrified, ailmentStateDuration);
            }
            else
            {
                Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 20f);
                float closestDistance = Mathf.Infinity;
                Transform closestEnemy = null;

                foreach (Collider2D col in cols)
                {
                   Enemy enemy = col.GetComponent<Enemy>();

                    float distToNextEnemy = Vector2.Distance(transform.position, col.transform.position);
                    if (enemy != null && distToNextEnemy > 1f) //ignore own enemy prefab
                    {
                        float currentDistance = Vector2.Distance(transform.position, enemy.transform.position);
                        if (currentDistance < closestDistance)
                        {
                            closestDistance = currentDistance;
                            closestEnemy = enemy.transform;
                        }
                    }

                    if(closestEnemy == null)
                    {
                       closestEnemy = transform;
                    }
                }

                GameObject lightningbolt = Instantiate(lightningBoltPrefab, transform.position, Quaternion.identity);
                lightningbolt.GetComponent<Thunder_Controls>().
                    SetupBoltAttributes(closestEnemy.GetComponent<CharacterStats>(), lightningDamage);

            }
            

        }     
        
    }
    public void SetupIgniteDamage(int _ignitionDamage) => ignitionDamage = _ignitionDamage;
    public void SetupLightningDamage(int _lightningDamage) => ignitionDamage = _lightningDamage;

    private int CalculateCritDamage(int _damage)
    {
        float totalCritValue = (critDamage.GetValue() + strength.GetValue())/100f;
        float critDamageValue = totalCritValue * _damage;
        critDamageValue = Mathf.RoundToInt(critDamageValue);
        return (int)critDamageValue;
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + "is dead!");
        isDead = true;
    }
}
