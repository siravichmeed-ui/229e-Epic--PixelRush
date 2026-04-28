using UnityEngine;

public class ArmProjectile : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;

    /*[Header("Homing")]
    public float homingStrength = 0.1f; // ยิ่งมากยิ่งเลี้ยวแรง*/

    [Header("Bounce")]
    public int maxBounce = 10; // เด้งได้กี่ครั้ง

    [Header("Lifetime")]
    public float lifeTime = 3f;

    private Vector2 direction;
    private Transform target;
    private Boss boss;

    private int bounceCount = 0;

    // ================= INIT =================
    public void Init(Boss b, Transform t)
    {
        boss = b;
        target = t;

        direction = (target.position - transform.position).normalized;
    }

    void Start()
    {
        Invoke(nameof(Return), lifeTime);
    }

    void Update()
    {
        Homing();
        Move();
        CheckBounce();
    }

    // ================= HOMING =================
    void Homing()
    {
        if (target == null) return;

        Vector2 targetDir = ((Vector2)target.position - (Vector2)transform.position).normalized;

        
/*direction = Vector2.Lerp(direction, targetDir, homingStrength * Time.deltaTime);*/
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // ================= MOVE =================
    void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    // ================= BOUNCE =================
    void CheckBounce()
    {
        if (Camera.main == null) return;

        Vector3 view = Camera.main.WorldToViewportPoint(transform.position);

        bool bounced = false;

        // ซ้าย-ขวา
        if (view.x <= 0f || view.x >= 1f)
        {
            direction.x *= -1;
            bounced = true;
        }

        // ล่าง-บน
        if (view.y <= 0f || view.y >= 1f)
        {
            direction.y *= -1;
            bounced = true;
        }

        if (bounced)
        {
            bounceCount++;

            // กันเด้งรัวติดขอบ
            transform.position += (Vector3)direction * 0.2f;

            if (bounceCount >= maxBounce)
            {
                Return();
            }
        }
    }

    // ================= RETURN =================
    void Return()
    {
        if (boss != null)
        {
            boss.ReturnArm();
            boss = null;
        }

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (boss != null)
            boss.ReturnArm();
    }

    // ================= HIT =================
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController p = other.GetComponent<PlayerController>();
            if (p != null)
                p.TakeDamage(1);

            Return();
        }
    }
}