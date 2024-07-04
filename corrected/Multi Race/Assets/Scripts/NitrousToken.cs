using UnityEngine;

public class NitrousToken : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the car's nitrous system and add nitrous
            other.GetComponent<CarNitrousSystem>().AddNitrous(10f); // Add 10 units of nitrous
            Destroy(gameObject); // Destroy the token
        }
    }
}

