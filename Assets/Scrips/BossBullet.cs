using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 8f;
    private Vector2 direction;

    void Start()
    {
        Destroy(gameObject, 5f); // 👈 ตรงนี้
    }

    public void SetTarget(Transform target)
    {
        direction = (target.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: ทำดาเมจ player (ถ้ามีระบบเลือด)

            Destroy(gameObject); // 👈 กระสุนหาย
        }
    }
}