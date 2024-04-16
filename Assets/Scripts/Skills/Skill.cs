using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] private float coolDownTime = 1f;
    private float coolDownTimer;



    private void Start()
    {
        coolDownTimer = coolDownTime;

    }

    private void Update()
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
