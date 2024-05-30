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

    public int currentHealth;


    protected virtual void Start()
    {
        critDamage.SetBaseValue(150);
        currentHealth = maxHealth.GetValue();
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
    }

    private int FilterDamageWithArmor(CharacterStats targetStats, int totalDamage)
    {
        totalDamage -= targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool CanTargetAvoidAttack(CharacterStats targetStats)
    {
        int totalEvasion = targetStats.evasion.GetValue() + targetStats.agility.GetValue();
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
