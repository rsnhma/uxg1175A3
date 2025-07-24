using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Optional")]
    public GameObject gameOverUI; // Assign if you have a UI popup (Canvas)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (KeycardPickup.hasKeycard)
        {
            Debug.Log("Player has the keycard. Level complete!");

            if (gameOverUI != null)
            {
                gameOverUI.SetActive(true);
                Time.timeScale = 0f; // pause game
            }
            else
            {
                // Or just load next level immediately
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        else
        {
            Debug.Log("Exit locked. Find the keycard first!");
        }
    }
}
