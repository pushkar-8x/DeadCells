using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField]private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration = 5f;

    [Header("Crystal Explode")]
    [SerializeField] private bool canExplode = false;
    [SerializeField] private float explosionRange = 3f;
    [SerializeField] private bool canGrow;
    [SerializeField] private float growSpeed = 5f;

    [Header("Crystal Movement")]
    [SerializeField] private bool canMove = false;
    [SerializeField] private float moveSpeed = 10f;

    private GameObject currentCrystal;

    public override void UseSkill()
    {
        base.UseSkill();

        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            Crystal_SkillController _skillController = currentCrystal.GetComponent<Crystal_SkillController>();
            _skillController.SetupCrystal(crystalDuration , canExplode ,explosionRange , canMove , moveSpeed ,
                canGrow , growSpeed , FindClosestEnemy(currentCrystal.transform));
        }
        else
        {
            if (canMove) return;

            Vector2 playerPos = player.transform.position;
            player.transform.position =  currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;
            currentCrystal.GetComponent<Crystal_SkillController>()?.FinishCrystalAbility();
        }
    }
}
