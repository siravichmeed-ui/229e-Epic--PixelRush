using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [Header("UI (auto assign)")]
    private GameObject pauseUI;
    private GameObject settingsUI;

    private bool isPaused = false;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

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
        // รีเซ็ตทุกครั้ง
        isPaused = false;
        Time.timeScale = 1f;

        // ❗ ล้าง reference เก่า (สำคัญมาก)
        pauseUI = null;
        settingsUI = null;
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("ESC pressed");

            // ถ้าอยู่ใน Settings → ปิดก่อน
            if (settingsUI != null && settingsUI.activeSelf)
            {
                CloseSettings();
                return;
            }

            TogglePause();
        }
    }

    // ================= REGISTER UI =================
    public void RegisterPauseUI(GameObject ui)
    {
        pauseUI = ui;
        pauseUI.SetActive(false);
    }

    public void RegisterSettingsUI(GameObject ui)
    {
        settingsUI = ui;
        settingsUI.SetActive(false);
    }

    // ================= PAUSE =================
    public void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        if (pauseUI == null)
        {
            Debug.LogError("PauseUI not registered!");
            return;
        }

        isPaused = true;
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseUI != null)
            pauseUI.SetActive(false);
    }

    // ================= SETTINGS =================
    public void OpenSettings()
    {
        if (settingsUI != null)
            settingsUI.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsUI != null)
            settingsUI.SetActive(false);
    }
}