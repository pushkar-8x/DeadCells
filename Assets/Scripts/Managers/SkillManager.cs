using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public Dash_Skill dashSkill { get; set; }

    public Clone_Skill cloneSkill { get; set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
    }

    private void Start()
    {
        dashSkill = GetComponent<Dash_Skill>();
        cloneSkill =  GetComponent<Clone_Skill>();
    }

}
