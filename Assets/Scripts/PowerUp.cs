using UnityEngine;

public class PowerUp : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("out"))
        {
            Destroy(gameObject);
        }
    }
}
