using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform player;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform laserOrigin;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private LineRenderer laserLine;

    [Header("Screen Lock")]
    [SerializeField] private float offsetX = -2f;
    [SerializeField] private float yMin = 0.3f, yMax = 0.7f, ySpeed = 1.2f;

    [Header("Stats")]
    [SerializeField] private int maxHP = 50;
    private int hp;

    [Header("Attack")]
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private float laserCooldown = 5f;

    private float fireTimer;
    private float laserTimer;
    private float y;

    private enum Phase { Phase1, Phase2, Phase3 }
    private Phase phase = Phase.Phase1;

    void Start()
    {
        hp = maxHP;
        y = 0.5f;

        if (laserLine != null)
            laserLine.enabled = false;
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
        laserTimer += Time.deltaTime;

        // ยิงธรรมดา
        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            anim.SetTrigger("attack");
        }

        // เลเซอร์ (เฉพาะ phase 2/3)
        if (phase != Phase.Phase1 && laserTimer >= laserCooldown)
        {
            laserTimer = 0f;
            anim.SetTrigger("laser");
        }
    }

    // ================= SHOOT (เรียกจาก Animation Event) =================
    public void Shoot()
    {
        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        BossBullet bullet = b.GetComponent<BossBullet>();
        if (bullet != null)
        {
            bullet.SetTarget(player);
        }
    }

    // ================= LASER (เรียกจาก Animation Event) =================
    public void AE_LaserStart()
    {
        StartCoroutine(LaserRoutine());
    }

    IEnumerator LaserRoutine()
    {
        float duration = (phase == Phase.Phase3) ? 2.5f : 1.5f;

        if (laserLine == null) yield break;

        laserLine.enabled = true;

        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;

            Vector3 start = laserOrigin.position;
            Vector3 dir = Vector3.left;
            float dist = 20f;

            laserLine.SetPosition(0, start);
            laserLine.SetPosition(1, start + dir * dist);

            // ตรวจโดน player
            RaycastHit2D hit = Physics2D.Raycast(start, dir, dist);
            if (hit && hit.collider.CompareTag("Player"))
            {
                // TODO: ทำดาเมจ player
            }

            yield return null;
        }

        laserLine.enabled = false;
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