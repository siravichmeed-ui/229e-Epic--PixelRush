using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] characters;

    void Start()
    {
        int index = CharacterSelector.selectedCharacter;
        Instantiate(characters[index], Vector3.zero, Quaternion.identity);
    }
}