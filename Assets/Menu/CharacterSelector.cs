using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static int selectedCharacter = 0;

    public void SelectCharacter(int index)
    {
        selectedCharacter = index;
        Debug.Log("Selected: " + index);
    }
}