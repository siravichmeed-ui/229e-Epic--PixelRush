using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class RockProjectile : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 1;

    [Header("Ground")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // 👉 ชน Player
        if (other.collider.CompareTag("Player"))
        {
            PlayerController player = other.collider.GetComponent<PlayerController>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
            return;
        }

        // 👉 ชนพื้น
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}