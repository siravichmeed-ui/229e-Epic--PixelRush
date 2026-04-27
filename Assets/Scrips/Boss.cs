using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform player;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Screen Lock")]
    [SerializeField] private float offsetX = -2f;
    [SerializeField] private float yMin = 0.3f, yMax = 0.7f, ySpeed = 1.2f;

    [Header("Stats")]
    [SerializeField] private int maxHP = 50;
    private int hp;

    [Header("Attack")]
    [SerializeField] private float fireRate = 1.5f;

    private float fireTimer;
    private float y;

    private enum Phase { Phase1, Phase2, Phase3 }
    private Phase phase = Phase.Phase1;

    void Start()
    {
        hp = maxHP;
        y = 0.5f;
    }

    void Update()
    {
        LockToScreen();
        FlipToPlayer();
        UpdatePhase();
        HandleAttack();
    }

    // ================= MOVE =================
    void LockToScreen()
    {
        y = Mathf.PingPong(Time.time * ySpeed, yMax - yMin) + yMin;

        Vector3 screenPos = new Vector3(1f, y, 0);
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(screenPos);

        worldPos.z = 0;
        worldPos.x += offsetX;

        transform.position = worldPos;
    }

    void FlipToPlayer()
    {
        if (!player) return;
        sr.flipX = player.position.x < transform.position.x;
    }

    // ================= PHASE =================
    void UpdatePhase()
    {
        float t = (float)hp / maxHP;

        if (t <= 0.3f && phase != Phase.Phase3)
        {
            phase = Phase.Phase3;
            anim.SetBool("isPhase3", true);
        }
        else if (t <= 0.6f && phase == Phase.Phase1)
        {
            phase = Phase.Phase2;
            anim.SetBool("isPhase2", true);
        }
    }

    // ================= ATTACK =================
    void HandleAttack()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            anim.SetTrigger("attack"); // ยิงผ่าน animation
        }
    }

    // ================= SHOOT (Animation Event) =================
    public void Shoot()
    {
        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        BossBullet bullet = b.GetComponent<BossBullet>();
        if (bullet != null)
        {
            bullet.SetTarget(player);
        }
    }

    // ================= DAMAGE =================
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        anim.SetTrigger("hit");

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetTrigger("die");
        Destroy(gameObject, 1.5f);
    }
}