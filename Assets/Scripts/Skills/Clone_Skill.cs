using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] GameObject clonePrefab;
    [SerializeField] private float cloneDuration = 1.5f;
    [SerializeField] bool canAttack = true;

    public void CreateClone(Transform transform , Vector2 offset)
    {
        GameObject clone  = Instantiate(clonePrefab);
        clone.GetComponent<CloneSkill_Controller>().SetupClone(transform , cloneDuration , canAttack , offset , FindClosestEnemy(clone.transform));

    }
}
