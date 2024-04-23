using System.Collections.Generic;
using UnityEngine;

public class Blackhole_SkillController : MonoBehaviour
{
    [SerializeField] private float maxBlackholeRadius = 20f;
    [SerializeField] private float growSpeed = 2.0f;
    [SerializeField] private bool canGrow;

    public List<Transform> targets = new List<Transform>();

    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale,
                new Vector2(maxBlackholeRadius, maxBlackholeRadius), growSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null)
        {
            targets.Add(collision.transform);
        }
    }
}
