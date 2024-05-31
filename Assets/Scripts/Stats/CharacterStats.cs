using UnityEngine;

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
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    private bool isIgnited;//does damage over time
    private bool isChilled;//reduces armor by 20%
    private bool isShocked;//increases evasion by 20%

    [SerializeField] float ignitionStateDuration = 5f;
    [SerializeField] float ignitionDamageCoolDown = 1f;


    private float ignitionStateTimer;
    private float ignitionDamageTimer;
    private int ignitionDamage;

    private float chilledStateTimer;
    private float shockedStateTimer;

    public int currentHealth;


    protected virtual void Start()
    {
        critDamage.SetBaseValue(150);
        currentHealth = maxHealth.GetValue();
    }

    private void Update()
    {
        ignitionStateTimer  -= Time.deltaTime;
        chilledStateTimer -= Time.deltaTime;
        shockedStateTimer -= Time.deltaTime;
        ignitionDamageTimer -= Time.deltaTime;


        if(chilledStateTimer <= 0)
        {
            isChilled = false;
        }

        if(shockedStateTimer <= 0)
        {
            isShocked = false;
        }

        if (ignitionStateTimer <= 0)
        {
            isIgnited = false;
        }
        if (ignitionDamageTimer <= 0 && isIgnited)
        {
            Debug.Log("Burning ! " + ignitionDamage);
            currentHealth -= ignitionDamage;
            if(currentHealth <= 0)
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
        //targetStats.TakeDamage(totalDamage);
        ApplyMagicDamage(targetStats);

        int fireDamageValue =fireDamage.GetValue();
        int iceDamageValue =iceDamage.GetValue();
        int lightningDamageValue =lightningDamage.GetValue();

        if(Mathf.Max(fireDamageValue, iceDamageValue, lightningDamageValue) <= 0)
        {
            return ;
        }

        bool canApplyIgnite = fireDamageValue > iceDamageValue && fireDamageValue > lightningDamageValue;
        bool canApplyChill = iceDamageValue > fireDamageValue && iceDamageValue > lightningDamageValue;
        bool canApplyShock = lightningDamageValue > fireDamageValue && lightningDamageValue > iceDamageValue;

        while(!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if(Random.value < 0.2f && fireDamageValue > 0)
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


        if(canApplyIgnite)
        {
            targetStats.SetupIgniteDamage(Mathf.RoundToInt(fireDamageValue * 0.2f));
        }

        targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

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
        currentHealth -= _damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());
        Debug.Log(gameObject.name + " takes " + _damage + " damage and has " + currentHealth + " health left.");
        if (currentHealth == 0)
        {
            Die();
        }
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

    private void ApplyMagicDamage(CharacterStats targetStats)
    {
        int totalMagicDamage = fireDamage.GetValue() + iceDamage.GetValue() + lightningDamage.GetValue() + intelligence.GetValue();
        totalMagicDamage = ApplyTargetResistance(targetStats, totalMagicDamage);
        targetStats.TakeDamage(totalMagicDamage);
    }

    private static int ApplyTargetResistance(CharacterStats targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= targetStats.magicResistance.GetValue() + targetStats.intelligence.GetValue();
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    private void ApplyAilments(bool isIgnited, bool isChilled, bool isShocked)
    {
        if(this.isIgnited || this.isChilled || this.isShocked)
        {
            return;
        }

        if(isIgnited)
        {
            ignitionStateTimer = ignitionStateDuration;
            this.isIgnited = isIgnited;
        }
        if(isChilled)
        {
            this.isChilled = isChilled;
        }
        if(isShocked)
        {
            this.isShocked = isShocked;
        }     
        
    }
    public void SetupIgniteDamage(int _ignitionDamage) => ignitionDamage = _ignitionDamage;

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
    }
}
