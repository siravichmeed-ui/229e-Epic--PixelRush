using UnityEngine;
using System.Collections;

public class PatternSpawner : MonoBehaviour
{
    [Header("Pattern")]
    public PatternData[] easy;
    public PatternData[] medium;
    public PatternData[] hard;

    [Header("Spawn")]
    public float spawnX = 10f;

    [Header("Boss")]
    public GameObject bossPrefab;
    public float bossDistance = 300f;
    public Vector2 bossSpawnPosition = new Vector2(10f, 0f);

    [Header("Item")]
    public GameObject itemPrefab;
    public float itemDelay = 2f;
    public float[] itemHeights;

    private bool bossSpawned = false;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float distance = GameManager.Instance.distance;

            if (!bossSpawned && distance >= bossDistance)
            {
                SpawnBoss();
                yield break;
            }

            PatternData pattern = GetPattern(distance);

            yield return StartCoroutine(SpawnPattern(pattern, distance));

            yield return new WaitForSeconds(1f);
        }
    }

    PatternData GetPattern(float distance)
    {
        if (distance < 100f)
            return easy[Random.Range(0, easy.Length)];

        if (distance < 200f)
            return medium[Random.Range(0, medium.Length)];

        return hard[Random.Range(0, hard.Length)];
    }

    IEnumerator SpawnPattern(PatternData pattern, float distance)
    {
        foreach (float y in pattern.spawnHeights)
        {
            GameObject prefab = pattern.obstacles[Random.Range(0, pattern.obstacles.Length)];

            Vector2 pos = new Vector2(spawnX, y);

            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                float speed = 3f + distance * 0.05f;
                rb.velocity = Vector2.left * speed;
            }

            yield return new WaitForSeconds(pattern.delay);
        }
    }

    void SpawnBoss()
    {
        bossSpawned = true;

        Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity);

        GameManager.Instance.EnterBossPhase();

        StartCoroutine(ItemLoop());
    }

    IEnumerator ItemLoop()
    {
        while (true)
        {
            if (Boss.Instance == null || Boss.Instance.IsDead())
                yield break;

            SpawnItem();

            yield return new WaitForSeconds(itemDelay);
        }
    }

    void SpawnItem()
    {
        float y = itemHeights[Random.Range(0, itemHeights.Length)];

        Vector2 pos = new Vector2(spawnX, y);

        GameObject obj = Instantiate(itemPrefab, pos, Quaternion.identity);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = Vector2.left * 5f;
        }
    }
}