using UnityEngine;

public class ItemDamageBoss : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // เล่น animation player
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.PlayAttack();
        }

        // ทำดาเมจ boss
        if (Boss.Instance != null)
        {
            Boss.Instance.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}