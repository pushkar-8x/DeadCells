using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill : Skill
{
    [Header("Skill Info")]
    [SerializeField] Blackhole_SkillController blackHolePrefab;
    [SerializeField] private float maxBlackholeRadius = 20f;
    [SerializeField] private float blackHoleDuration = 3f;
    [SerializeField] private float growSpeed = 2.0f;
    [SerializeField] private float shrinkSpeed = 5.0f;
    [SerializeField] private float cloneAttackCoolDown = 1f;
    [SerializeField] private float cloneOffset = 2f;
    [SerializeField] private int amountOfAttacks = 5;

    Blackhole_SkillController currentBlackHole;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();

    }

    public override void UseSkill()
    {
        base.UseSkill();
        currentBlackHole = Instantiate(blackHolePrefab , player.transform.position, Quaternion.identity);
        currentBlackHole.SetupBlackHole(maxBlackholeRadius, growSpeed, shrinkSpeed, cloneAttackCoolDown, amountOfAttacks , blackHoleDuration);
    }

    protected override void Start()
    {
        base.Start();
       
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool IsBlackHoleFinished()
    {
        if (!currentBlackHole) return false;
        if(currentBlackHole.playerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }

        return false;
    }
}
