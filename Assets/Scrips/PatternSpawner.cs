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

    [Header("Difficulty")]
    public float gameTime;

    [Header("Boss")]
    public GameObject bossPrefab;
    public float bossTime = 60f;

    private bool bossSpawned = false;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
        gameTime += Time.deltaTime;
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            // 👉 เช็ค boss
            if (!bossSpawned && gameTime >= bossTime)
            {
                SpawnBoss();
                yield break;
            }

            PatternData pattern = GetPattern();

            yield return StartCoroutine(SpawnPattern(pattern));

            yield return new WaitForSeconds(1f);
        }
    }

    PatternData GetPattern()
    {
        if (gameTime < 20f)
            return easy[Random.Range(0, easy.Length)];

        if (gameTime < 40f)
            return medium[Random.Range(0, medium.Length)];

        return hard[Random.Range(0, hard.Length)];
    }

    IEnumerator SpawnPattern(PatternData pattern)
    {
        foreach (float y in pattern.spawnHeights)
        {
            GameObject prefab = pattern.obstacles[Random.Range(0, pattern.obstacles.Length)];

            Vector2 pos = new Vector2(spawnX, y);

            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                float speed = 3f + gameTime * 0.1f;
                rb.velocity = Vector2.left * speed;
            }

            yield return new WaitForSeconds(pattern.delay);
        }
    }

    void SpawnBoss()
    {
        Instantiate(bossPrefab, new Vector2(spawnX, 0), Quaternion.identity);
    }
}