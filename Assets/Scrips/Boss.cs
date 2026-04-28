using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Arm (ตัวที่ติดตัว)")]
    [SerializeField] private GameObject armObject;   // Arm_Right (ใน Hierarchy)
    [SerializeField] private Animator armAnim;       // Animator ของแขน

    [Header("Shoot Point (ตัวเล็งจริง)")]
    [SerializeField] private Transform shootPos;     // จุดปลายแขน (หมุนตัวนี้)

    [Header("Projectile Arm")]
    [SerializeField] private GameObject armPrefab;   // Prefab แขน (ไอคอนสีน้ำเงิน)

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 2f;

    [Header("Aim")]
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float minAngle = -80f;
    [SerializeField] private float maxAngle = 80f;
    [SerializeField] private float angleOffset = 0f; // ถ้าสไปรท์ไม่หันขวา ใส่ 90 หรือ -90

    [Header("Phase")]
    [SerializeField] private int maxHP = 50;
    private int currentHP;

    private float attackTimer;
    private int phase = 1;
    private bool isArmOut = false;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        if (player == null) return;

        HandleAttack();
        UpdatePhase();
    }

    // ใช้ LateUpdate กัน animation มาทับ
    void LateUpdate()
    {
        AimShootPos();
    }

    // ================= AIM =================
    void AimShootPos()
    {
        if (isArmOut) return; // ตอนยิงอยู่ไม่ต้องหมุน

        Vector2 dir = player.position - shootPos.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        angle += angleOffset;
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        Quaternion targetRot = Quaternion.Euler(0, 0, angle);

        shootPos.rotation = Quaternion.Lerp(
            shootPos.rotation,
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

            // เรียก animation แขน
            if (armAnim != null)
                armAnim.SetTrigger("attack");
        }
    }

    // 👉 เรียกจาก Animation Event (บน Arm)
    public void ShootArm()
    {
        if (isArmOut) return;

        isArmOut = true;

        armObject.SetActive(false);

        GameObject armObj = Instantiate(armPrefab, shootPos.position, Quaternion.identity);

        ArmProjectile proj = armObj.GetComponent<ArmProjectile>();

        if (proj != null)
        {
            proj.Init(this, player); // 👈 ส่ง player ไป
        }
    }

    // 👉 ให้แขนกลับ
    public void ReturnArm()
    {
        isArmOut = false;
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