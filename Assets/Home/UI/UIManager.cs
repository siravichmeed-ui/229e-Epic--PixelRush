using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject popupUI;

    public void ShowUI()
    {
        popupUI.SetActive(true);
    }

    public void HideUI()
    {
        popupUI.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
}