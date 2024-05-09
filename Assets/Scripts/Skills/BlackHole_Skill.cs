using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill : Skill
{
    [Header("Skill Info")]
    [SerializeField] Blackhole_SkillController blackHolePrefab;
    [SerializeField] private float maxBlackholeRadius = 20f;
    [SerializeField] private float growSpeed = 2.0f;
    [SerializeField] private float shrinkSpeed = 5.0f;
    [SerializeField] private float cloneAttackCoolDown = 1f;
    [SerializeField] private float cloneOffset = 2f;
    [SerializeField] private int amountOfAttacks = 5;


    public override bool CanUseSkill()
    {
        return base.CanUseSkill();

    }

    public override void UseSkill()
    {
        base.UseSkill();
        Blackhole_SkillController blackHole = Instantiate(blackHolePrefab , player.transform.position, Quaternion.identity);

        blackHole.SetupBlackHole(maxBlackholeRadius, growSpeed, shrinkSpeed, cloneAttackCoolDown, amountOfAttacks);
    }

    protected override void Start()
    {
        base.Start();
       
    }

    protected override void Update()
    {
        base.Update();
    }
}
