using UnityEngine;

public class EnemyThrower : MonoBehaviour
{
    [Header("Throw")]
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float throwCooldown = 2f;

    [Header("Throw Force")]
    [SerializeField] private Vector2 throwForce = new Vector2(2f, 5f); // 👉 แรงโยน

    private float timer;
    
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= throwCooldown)
        {
            timer = 0f;
            ThrowRock();
        }
    }

    void ThrowRock()
    {
        if (rockPrefab == null || throwPoint == null) return;

        GameObject rock = Instantiate(rockPrefab, throwPoint.position, Quaternion.identity);

        Rigidbody2D rb = rock.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // 👇 สุ่มแรง
            float randomX = Random.Range(1f, 3f);   // ซ้าย-ขวา
            float randomY = Random.Range(4f, 7f);   // ความสูง

            Vector2 force = new Vector2(randomX, randomY);

            rb.AddForce(force, ForceMode2D.Impulse);

            // หมุนให้เท่
            rb.angularVelocity = Random.Range(-200f, 200f);
        }
    }
}