using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChaser : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private string targetTag = "Player";

    [Header("Movement")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private bool followContinuously = false;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 8f;

    private Rigidbody2D rb;
    private Vector2 direction;
    private bool hasHit = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (followContinuously)
            CalculateDirection();

        rb.velocity = direction * speed;
    }

    void CalculateDirection()
    {
        if (target == null) return;
        direction = ((Vector2)target.position - rb.position).normalized;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (hasHit) return;

        if (other.collider.CompareTag(targetTag))
        {
            hasHit = true;

            PlayerController player = other.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1);
            }

            GetComponent<Collider2D>().enabled = false;
        }
    }
}