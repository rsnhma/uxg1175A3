using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverUI; // Assign in Inspector

    private GameObject player;
    private Sprite playersprite;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Hide game over UI at start
        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        // Reset keycard collection
        KeycardPickup.hasKeycard = false;
    }

    // Called by CharacterSpawner after player is spawned
    public void SetPlayer(GameObject playerInstance)
    {
        player = playerInstance;

        // Optional: update sprite or other setup if needed
        // For example, cache PlayerStats or subscribe to health events here
    }

    public void TriggerGameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("Game Over triggered.");
        }
    }
}