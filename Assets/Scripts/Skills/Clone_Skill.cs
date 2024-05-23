using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] GameObject clonePrefab;
    [SerializeField] private float cloneDuration = 1.5f;
    [SerializeField] bool canAttack = true;

    [SerializeField] bool useCloneOnDashStart = false;
    [SerializeField] bool useCloneOnDashEnd = false;
    [SerializeField] bool useCloneOnCounterAttack = false;
    [SerializeField] float spawnDelayOnCounter = 0.5f;

    [Header("Duplicate Clone")]
    [SerializeField] bool canDuplicateClone = false;
    [SerializeField] float duplicateChance = 35f;
    public bool createCrystalInsteadOfClone = false;


    public void CreateClone(Transform currTransform , Vector2 offset)
    {
        if(createCrystalInsteadOfClone)
        {
            player.skillManager.crystalSkill.CreateCrystal();
            return;
        }

        GameObject clone  = Instantiate(clonePrefab);
        clone.GetComponent<CloneSkill_Controller>().SetupClone(currTransform , cloneDuration , canAttack , offset ,
            FindClosestEnemy(clone.transform), canDuplicateClone , duplicateChance);

    }

    public void UseCloneOnDashStart()
    {
        if(useCloneOnDashStart)
        {
            CreateClone(player.transform , Vector2.zero);
        }
    }

    public void UseCloneOnDashEnd()
    {
        if (useCloneOnDashEnd)
        {
            CreateClone(player.transform, Vector2.zero);
        }
    }

    public void CreateCloneOnCounterAttack(Transform _transform)
    {
        if (useCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelayRoutine(_transform, new Vector2(1f * player.faceDirection, 0.2f), spawnDelayOnCounter));
        }
    }

    private IEnumerator CreateCloneWithDelayRoutine(Transform _transform , Vector2 offset , float delay)
    {
        yield return new WaitForSeconds(delay);
        CreateClone(_transform , offset);
    }
}
