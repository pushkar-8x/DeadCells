using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Skill Attributes")]
    [SerializeField] GameObject swordPrefab;
    [SerializeField] Vector2 launchForce;

    [SerializeField] float swordGravity;

    [Header("Dots Info")]
    [SerializeField] GameObject dotsPrefab;
    [SerializeField] int dotsCount = 10;
    [SerializeField] float spaceBwDots = 2f;
    [SerializeField] Transform dotsParent;

    private GameObject[] dots; 

    private Vector2 finalDirection;


    protected override void Start()
    {
        base.Start();
        GenerateDots();
    }

    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDirection = new Vector2 (GetAimDirection().normalized.x * launchForce.x , GetAimDirection().normalized.y * launchForce.y);
        }

        if(Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dotsCount; i++)
            {
                dots[i].transform.position = PositionDots(i *  spaceBwDots);
            }
        }

        
    }

    private Vector2 GetAimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 dir = mousePosition - playerPosition;
        return dir;
    }

    public void CreateSword()
    {
        GameObject sword  = Instantiate(swordPrefab , player.transform.position , transform.rotation);
        player.AssignSword(sword);
        sword.GetComponent<SwordSkill_Controller>().SetupSword(finalDirection, swordGravity , player);

        ActivateDots(false);
    }

    private void GenerateDots()
    {
        dots = new GameObject[dotsCount];
        for (int i = 0; i < dotsCount; i++)
        {
            dots[i] = Instantiate(dotsPrefab , player.transform.position , Quaternion.identity , dotsParent);
            dots[i].SetActive(false);
        }
    }

    public void ActivateDots(bool enable)
    {
        for (int i = 0; i < dotsCount; i++)
        {
            dots[i].SetActive(enable);
        }
    }

    private Vector2 PositionDots(float t)
    {
        Vector2 dotsPos = (Vector2)player.transform.position + 
            new Vector2(GetAimDirection().normalized.x * launchForce.x, GetAimDirection().normalized.y * launchForce.y) * t +
            0.5f * (Physics2D.gravity * swordGravity) * t * t;

        return dotsPos;
    }

}
