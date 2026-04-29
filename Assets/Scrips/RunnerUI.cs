using UnityEngine;

public class RunnerUI : MonoBehaviour
{
    public RectTransform playerIcon;
    public RectTransform line;

    public float maxDistance = 300f; // ระยะถึง boss

    void Update()
    {
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
}