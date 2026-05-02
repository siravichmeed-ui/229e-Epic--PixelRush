using UnityEngine;

public class PausePanel : MonoBehaviour
{
    void Start()
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.RegisterPauseUI(gameObject);
        }
    }
}