using UnityEngine;

public class PotionThrow : MonoBehaviour
{
    public GameObject potionPrefab;         // Assign your prefab here
    public Transform throwPoint;           // Where the potion spawns from
    public float throwCooldown = 1.5f;     // Time between throws

    private float lastThrowTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastThrowTime + throwCooldown)
        {
            ThrowPotion();
            lastThrowTime = Time.time;
        }
    }

    void ThrowPotion()
    {
        Instantiate(potionPrefab, throwPoint.position, throwPoint.rotation);
    }
}