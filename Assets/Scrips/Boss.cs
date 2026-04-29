using UnityEngine;

public class Boss : MonoBehaviour
{
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

    [Header("Phase")]
    [SerializeField] private int maxHP = 50;

    private int currentHP;
    private float attackTimer;
    private int phase = 1;
    private bool isArmOut = false;

    // ================= START =================
    void Start()
    {
        currentHP = maxHP;

        FindPlayer(); // 👈 หา player ตอนเริ่ม
    }

    // ================= UPDATE =================
    void Update()
    {
        // 👇 เผื่อ player spawn ทีหลัง
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

        if (obj != null)
        {
            player = obj.transform;
        }
        else
        {
            Debug.LogWarning("Boss: หา Player ไม่เจอ (เช็ค Tag)");
        }
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

    // ================= SHOOT =================
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

    // ================= RETURN ARM =================
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
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss Dead");
        Destroy(gameObject);
    }
}