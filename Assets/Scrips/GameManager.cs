using UnityEngine;
using UnityEngine.SceneManagement;

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
    public bool isBossPhase = false;

    void Awake()
    {
        Time.timeScale = 1f;

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

    // 🔥 เพิ่มอันนี้ (สำคัญมาก)
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetGame(); // 👈 รีเซ็ตทุกครั้งหลังโหลด
    }

    void Update()
    {
        if (!isGameRunning) return;

        distance += speed * Time.deltaTime;
        speed += speedIncreaseRate * Time.deltaTime;
    }

    // ================= CONTROL =================
    public void StopGame()
    {
        isGameRunning = false;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isGameRunning = true;
        Time.timeScale = 1f;
    }

    public void ResetGame()
    {
        distance = 0f;
        speed = 5f;
        speedIncreaseRate = 0.02f;

        isBossPhase = false;
        isGameRunning = true;

        Time.timeScale = 1f;
    }

    // ================= BOSS =================
    public void EnterBossPhase()
    {
        isBossPhase = true;
        speedIncreaseRate = 0f;
    }

    public void BossDefeated()
    {
        Debug.Log("Boss Cleared!");
        StopGame();
    }

    // ================= RESTART =================
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}