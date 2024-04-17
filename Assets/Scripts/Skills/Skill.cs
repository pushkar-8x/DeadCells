using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDownTime = 1f;
    protected float coolDownTimer;
    protected Player player;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        coolDownTimer = coolDownTime;

    }

    protected virtual void Update()
    {
        if(coolDownTimer > -1f) 
        coolDownTimer -= Time.deltaTime;
    }
    public virtual bool CanUseSkill()
    {
        if(coolDownTimer < 0f)
        {
            UseSkill();
            coolDownTimer = coolDownTime;
            return true;
        }

        Debug.Log("Skill on cooldown !");
        return false;
    }

    public virtual void UseSkill()
    {

    }


}
