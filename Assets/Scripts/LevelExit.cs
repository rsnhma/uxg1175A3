using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Optional")]
    public GameObject levelCompleteUI; // assign a different canvas here

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (KeycardPickup.hasKeycard)
        {
            Debug.Log("Player has the keycard. Level complete!");

            if (levelCompleteUI != null)
            {
                levelCompleteUI.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        else
        {
            Debug.Log("Exit locked. Find the keycard first!");
        }
    }
}
