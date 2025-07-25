using UnityEngine;

public class KeycardPickup : MonoBehaviour
{
    public static bool hasKeycard = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hasKeycard = true;
            Debug.Log("Keycard collected!");

            Destroy(gameObject); // remove keycard from the scene
        }
    }
}
