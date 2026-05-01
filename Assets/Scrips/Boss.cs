using UnityEngine;

public class Boss : MonoBehaviour
{
    public static Boss Instance;

    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Arm")]
    [SerializeField] private GameObject armObject;
    [SerializeField] private Animator armAnim;

    [Header("Shoot Point")]
    [SerializeField] private Transform shootPos;

    [Header("Projectile Arm")]
    [SerializeField] private GameObject armPrefab;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 2f;

    [Header("Aim")]
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float minAngle = -80f;
    [SerializeField] private float maxAngle = 80f;
    [SerializeField] private float angleOffset = 0f;

    [Header("HP")]
    [SerializeField] private int maxHP = 10;

    [SerializeField] private Animator anim;
    [SerializeField] private float destroyDelay = 1.5f;

    private int currentHP;
    private float attackTimer;
    private int phase = 1;
    private bool isArmOut = false;
    private bool isDead = false;

    // ================= INIT =================
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentHP = maxHP;
        FindPlayer();
    }

    // ================= UPDATE =================
    void Update()
    {
        if (isDead) return;

        if (player == null)
        {
            FindPlayer();
            return;
        }

        HandleAttack();
        UpdatePhase();
    }

    void LateUpdate()
    {
        AimShootPos();
    }

    // ================= FIND PLAYER =================
    void FindPlayer()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null) player = obj.transform;
    }

    // ================= AIM =================
    void AimShootPos()
    {
        if (isArmOut || shootPos == null || player == null) return;

        Vector2 dir = player.position - shootPos.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        angle += angleOffset;
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        Quaternion targetRot = Quaternion.Euler(0, 0, angle);

        shootPos.localRotation = Quaternion.Lerp(
            shootPos.localRotation,
            targetRot,
            Time.deltaTime * rotateSpeed
        );
    }

    // ================= ATTACK =================
    void HandleAttack()
    {
        if (isArmOut) return;

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            if (armAnim != null)
                armAnim.SetTrigger("attack");
        }
    }

    // 👉 Animation Event เรียกตัวนี้
    public void ShootArm()
    {
        if (isArmOut || shootPos == null || armPrefab == null) return;

        isArmOut = true;

        if (armObject != null)
            armObject.SetActive(false);

        GameObject armObj = Instantiate(armPrefab, shootPos.position, Quaternion.identity);

        ArmProjectile proj = armObj.GetComponent<ArmProjectile>();

        if (proj != null)
        {
            proj.Init(this, player);
        }
    }

    // 👉 เรียกจาก projectile ตอนกลับ
    public void ReturnArm()
    {
        isArmOut = false;

        if (armObject != null)
            armObject.SetActive(true);
    }

    // ================= PHASE =================
    void UpdatePhase()
    {
        float hpPercent = (float)currentHP / maxHP;

        if (hpPercent <= 0.6f && phase == 1)
        {
            phase = 2;
            attackCooldown = 1.2f;
        }

        if (hpPercent <= 0.3f && phase == 2)
        {
            phase = 3;
            attackCooldown = 0.7f;
        }
    }

    // ================= DAMAGE =================
    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHP -= dmg;

        Debug.Log("Boss HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    void Die()
    {
        isDead = true;

        Debug.Log("Boss Dead");

        // 👉 เล่น animation ตาย
        if (anim != null)
        {
            anim.SetTrigger("die");
        }

        // 👉 แจ้ง GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.BossDefeated();
        }

        // 👉 ทำลายหลัง animation
        Destroy(gameObject, destroyDelay);
    }
}