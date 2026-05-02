using UnityEngine;
using System.Collections;

public class RunnerUI : MonoBehaviour
{
    public RectTransform playerIcon;
    public RectTransform line;

    public float maxDistance = 300f;

    IEnumerator Start()
    {
        yield return null; // รอ GameManager reset ก่อน
        ResetUI();
    }

    void Update()
    {
        if (GameManager.Instance == null) return;
        if (Time.timeScale == 0f) return; // 👈 ใช้แทน isGameRunning

        MovePlayer();
    }

    void MovePlayer()
    {
        float distance = GameManager.Instance.distance;

        float t = Mathf.Clamp01(distance / maxDistance);

        float width = line.rect.width;

        Vector2 pos = playerIcon.anchoredPosition;
        pos.x = t * width;

        playerIcon.anchoredPosition = pos;
    }

    public void ResetUI()
    {
        Vector2 pos = playerIcon.anchoredPosition;
        pos.x = 0f;
        playerIcon.anchoredPosition = pos;
    }
}