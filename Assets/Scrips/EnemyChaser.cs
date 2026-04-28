using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyChaser : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private string targetTag = "Player";

    [Header("Movement")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private bool followContinuously = false;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 8f;

    private Rigidbody2D rb;
    private Collider2D col;

    private Vector2 direction;
    private bool hasHit = false;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        if (target == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag(targetTag);
            if (obj != null) target = obj.transform;
        }

        CalculateDirection();
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        CheckGround();

        if (followContinuously)
            CalculateDirection();

        // เดินเฉพาะตอนอยู่บนพื้น
        if (isGrounded)
        {
            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
        }
    }

    void CalculateDirection()
    {
        if (target == null) return;
        direction = ((Vector2)target.position - rb.position).normalized;
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundDistance, groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (hasHit) return;

        if (other.collider.CompareTag(targetTag))
        {
            hasHit = true;

            // ทำดาเมจ
            PlayerController player = other.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1);
            }

            // 👇 ทำให้ทะลุ player (แต่ยังชนพื้นได้)
            Physics2D.IgnoreCollision(other.collider, col);

            // (optional) กันชน player ซ้ำแน่ ๆ
            // Destroy(col, 0.1f);
        }
    }

    // Debug ground check
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}