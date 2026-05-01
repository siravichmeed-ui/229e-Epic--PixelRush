using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Distance")]
    public float distance = 0f;

    [Header("Speed")]
    public float speed = 5f;
    public float speedIncreaseRate = 0.02f;

    [Header("State")]
    public bool isGameRunning = true;

    [Header("Boss")]
    public bool isBossPhase = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!isGameRunning) return;

        // 👉 เพิ่มระยะ
        distance += speed * Time.deltaTime;

        // 👉 เพิ่มความเร็วเรื่อย ๆ
        speed += speedIncreaseRate * Time.deltaTime;
    }

    // ================= CONTROL =================
    public void StopGame()
    {
        isGameRunning = false;
    }

    public void ResumeGame()
    {
        isGameRunning = true;
    }

    public void ResetGame()
    {
        distance = 0f;
        speed = 5f;
        isBossPhase = false;
        isGameRunning = true;
    }

    // ================= BOSS =================
    public void EnterBossPhase()
    {
        isBossPhase = true;

        // 👉 หยุดเพิ่มความเร็ว (optional)
        speedIncreaseRate = 0f;
    }

    public void BossDefeated()
    {
        Debug.Log("Boss Cleared!");

        // 👉 จะหยุดเกม หรือให้ไปต่อก็ได้
        StopGame();
    }
}