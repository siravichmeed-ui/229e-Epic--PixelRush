using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: damage player ต่อเนื่อง
        }
    }
}
