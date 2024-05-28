using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    
    public int currentHealth;
    public Stat maxHealth;
    public Stat strength;
    public Stat damage;

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }

    public virtual void ApplyDamage(CharacterStats targetStats)
    {
        
        int totalDamage = strength.GetValue() + damage.GetValue();
        targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        
        currentHealth -= _damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());
        Debug.Log(gameObject.name + " takes " + _damage + " damage and has " + currentHealth + " health left.");
        if(currentHealth == 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + "is dead!");
    }
}
