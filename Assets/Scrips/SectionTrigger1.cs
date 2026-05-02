using UnityEngine;

public class SectionTrigger1 : MonoBehaviour
{
    public GameObject roadSection;
    private bool spawned = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!spawned && other.CompareTag("Player"))
        {
            spawned = true;

            Instantiate(
                roadSection,
                transform.parent.position + new Vector3(10f, 0, 0),
                Quaternion.identity
            );
        }
    }
} 
 