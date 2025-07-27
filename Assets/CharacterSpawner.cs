using UnityEngine;
using System.Collections.Generic;

public class CharacterSpawner : MonoBehaviour
{
    public List<GameObject> characterPrefabs; // Assign prefabs in Inspector
    public Transform spawnPoint;              // Where the player should spawn
    public Vector2 facingDirectionForSpawn = new Vector2(1, 0); // Default face right

    public WeaponUIManager weaponUIManager;

    void Start()
    {
        int index = CharManager.SelectedIndex;

        if (index >= 0 && index < characterPrefabs.Count)
        {
            // Instantiate the selected character prefab
            GameObject player = Instantiate(characterPrefabs[index], spawnPoint.position, Quaternion.identity);
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetPlayer(player);
            }

            // Set initial facing direction using method in PlayerMovement
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.SetInitialFacingDirection(facingDirectionForSpawn);
            }

            // Assign the camera to follow the new player
            CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
            if (camFollow != null)
            {
                camFollow.target = player.transform;
            }

            if (playerMovement != null)
            {
                playerMovement.weaponUIManager = weaponUIManager;
            }
        }
        else
        {
            Debug.LogWarning("Invalid selected character index");
        }
    }
}
