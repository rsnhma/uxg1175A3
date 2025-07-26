using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("UI References")]
    public GameObject levelCompleteUI;   // Assigned in Inspector
    public GameObject keycardWarningUI;  // <-- NEW: Assign your "Need keycard" popup
    public float warningDuration = 2f;   // Time to show the warning popup

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

            if (keycardWarningUI != null)
            {
                keycardWarningUI.SetActive(true);
                CancelInvoke(nameof(HideWarning)); // prevent overlap
                Invoke(nameof(HideWarning), warningDuration);
            }
        }
    }

    void HideWarning()
    {
        if (keycardWarningUI != null)
        {
            keycardWarningUI.SetActive(false);
        }
    }
}