using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    void Start()
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.RegisterSettingsUI(gameObject);
        }
    }
}