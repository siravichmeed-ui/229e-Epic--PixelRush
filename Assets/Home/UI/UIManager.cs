using UnityEngine;
using UnityEngine.SceneManagement;

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
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void GameMenu()
    {
        SceneManager.LoadScene(1);
    }
}