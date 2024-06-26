using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField]private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration = 5f;

    [Header("Crystal Mirage")]
    [SerializeField] private bool canSpawnCloneForCrystal = false;

    [Header("Crystal Explode")]
    [SerializeField] private bool canExplode = false;
    [SerializeField] private float explosionRange = 3f;
    [SerializeField] private bool canGrow;
    [SerializeField] private float growSpeed = 5f;

    [Header("Crystal Movement")]
    [SerializeField] private bool canMove = false;
    [SerializeField] private float moveSpeed = 10f;

    [Header("MultiCrystals")]
    [SerializeField] private bool canSpawnMultiCrystals = false;
    [SerializeField] private int maxStackAmount = 3;
    [SerializeField] private float multiCrystalCoolDown = 1f;
    [SerializeField] private float useTimeWindow = 2.5f;
    [SerializeField] private List<GameObject> multiCrystalsList = new List<GameObject>();

    public void CrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_SkillController>().ChooseRandomEnemyTarget();


    private GameObject currentCrystal;

    public override void UseSkill()
    {
        base.UseSkill();

        

        if (CanUseMultiCrystal())
            return;

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMove) return;

            Vector2 playerPos = player.transform.position;
            player.transform.position =  currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if(canSpawnCloneForCrystal)
            {
                SkillManager.instance.cloneSkill.CreateClone(currentCrystal.transform , Vector2.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_SkillController>()?.FinishCrystalAbility();
            }

            
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_SkillController _skillController = currentCrystal.GetComponent<Crystal_SkillController>();
        _skillController.SetupCrystal(crystalDuration, canExplode, explosionRange, canMove, moveSpeed,
            canGrow, growSpeed, FindClosestEnemy(currentCrystal.transform) , player);
    }

    private bool CanUseMultiCrystal()
    {
        if(canSpawnMultiCrystals)
        {
            if(multiCrystalsList.Count > 0)
            {
                 
                if(multiCrystalsList.Count == maxStackAmount)
                {
                    Invoke("ResetCrystalAbility", useTimeWindow);
                }

                coolDownTime = 0f;
                GameObject crystal = multiCrystalsList[multiCrystalsList.Count - 1];
                GameObject newCrystal = Instantiate(crystal, player.transform.position, Quaternion.identity);
                multiCrystalsList.Remove(crystal);
                newCrystal.GetComponent<Crystal_SkillController>().SetupCrystal(crystalDuration, canExplode, explosionRange, canMove, moveSpeed,
                canGrow, growSpeed, FindClosestEnemy(newCrystal.transform),player);

                if(multiCrystalsList.Count <= 0)
                {
                    coolDownTime = multiCrystalCoolDown;
                    RefillCrystal();
                }

                return true;
            }

            
        }
        return false;
    }

    private void RefillCrystal()
    {
        int amountToRefill = maxStackAmount - multiCrystalsList.Count;
        for (int i = 0; i < amountToRefill; i++)
        {
            multiCrystalsList.Add(crystalPrefab);
        }
    }

    private void ResetCrystalAbility()
    {
        if(coolDownTimer > 0f) { return; }

        coolDownTimer = multiCrystalCoolDown;
        RefillCrystal();
    }
}
