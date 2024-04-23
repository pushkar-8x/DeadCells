using UnityEngine;

public enum SwordType
{
    Normal,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    [Header("Skill Attributes")]
    [SerializeField] GameObject swordPrefab;
    [SerializeField] Vector2 launchForce;
    [SerializeField] private float regularSwordGravity = 5f;
    [SerializeField] private float swordReturnSpeed = 15f;
    [SerializeField] float freezeDuration = 1f;

    [Header("Bounce Info")]
    [SerializeField] private bool canBounce = true;
    [SerializeField] private int bounceCount = 4;
    [SerializeField] private float bounceGravity = 5;
    [SerializeField] private float bounceSpeed = 20f;

    [Header("Pierce Info")]
    [SerializeField] private int pierceCount = 3;
    [SerializeField] private float pierceGravity = 5;

    [Header("Spin Info")]
    [SerializeField] private bool canSpin = true;
    [SerializeField] private float spinDuration = 3f;
    [SerializeField] private float spinGravity = 1f;
    [SerializeField] private float maxSpinDistance = 10f;
    [SerializeField] private float hitCoolDown = 0.35f;

    [Header("Dots Info")]
    [SerializeField] GameObject dotsPrefab;
    [SerializeField] int dotsCount = 10;
    [SerializeField] float spaceBwDots = 2f;
    [SerializeField] Transform dotsParent;

    private GameObject[] dots;
    private float swordGravity;
    [SerializeField] private SwordType _swordType = SwordType.Normal;
    private Vector2 finalDirection;


    protected override void Start()
    {
        base.Start();
        SetupSwordGravity();
        GenerateDots();
        
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDirection = new Vector2(GetAimDirection().normalized.x * launchForce.x, GetAimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            SetupSwordGravity();///to be removed later just for debugging!
            for (int i = 0; i < dotsCount; i++)
            {
                dots[i].transform.position = PositionDots(i * spaceBwDots);
            }
        }
    }

    private void SetupSwordGravity()
    {
        switch(_swordType)
        {
            case SwordType.Normal:
                swordGravity = regularSwordGravity;
                break;
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                break;
            case SwordType.Spin:
                swordGravity = spinGravity;
                break;
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
        GameObject sword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        player.AssignSword(sword);
        var ssc = sword.GetComponent<SwordSkill_Controller>();
        SetupSwordGravity();

        switch (_swordType)
        {
            case SwordType.Bounce:
                ssc.SetupBounce(canBounce, bounceCount,bounceSpeed);
                break;

            case SwordType.Pierce:
                ssc.SetupPierce(pierceCount);
                break;

            case SwordType.Spin:
                ssc.SetupSpin(canSpin,spinDuration,maxSpinDistance,hitCoolDown);
                break;

            default:
                break;
        }
        ssc.SetupSword(finalDirection, swordGravity, player , freezeDuration , swordReturnSpeed);
        ActivateDots(false);
    }

    private void GenerateDots()
    {
        dots = new GameObject[dotsCount];
        for (int i = 0; i < dotsCount; i++)
        {
            dots[i] = Instantiate(dotsPrefab, player.transform.position, Quaternion.identity, dotsParent);
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
