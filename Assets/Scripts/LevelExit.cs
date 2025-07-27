using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("UI References")]
    public GameObject levelCompleteUI;    // Shows when level is complete
    public GameObject keycardWarningUI;   // Shows when trying to exit without keycard

    private bool levelCompleted = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (levelCompleted) return; // prevent re-enter

        if (KeycardPickup.hasKeycard)
        {
            Debug.Log("Player has the keycard. Level complete!");
            levelCompleted = true;
            GetComponent<BoxCollider2D>().enabled = false;

            if (levelCompleteUI != null)
            {
                levelCompleteUI.SetActive(true);
                Time.timeScale = 0f; // pause game
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        else
        {
            Debug.Log("Exit locked. Player needs keycard.");

            if (keycardWarningUI != null)
            {
                keycardWarningUI.SetActive(true);

                // Optional: auto-hide after 2 seconds
                Invoke(nameof(HideKeycardWarning), 2f);
            }
        }
    }

    void HideKeycardWarning()
    {
        if (keycardWarningUI != null)
            keycardWarningUI.SetActive(false);
    }
}
