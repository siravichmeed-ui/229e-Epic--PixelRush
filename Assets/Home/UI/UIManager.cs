using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] allUI;

    public void ShowUI(GameObject target)
    {
        foreach (GameObject ui in allUI)
        {
            ui.SetActive(false);
        }

        target.SetActive(true);
    }
}