using UnityEngine;

public class Beam : MonoBehaviour
{
    public float damagePerSecond = 5f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        EnemyBehaviour enemy = collision.GetComponent<EnemyBehaviour>();
        if (enemy != null)
        {
            float damagePerFrame = damagePerSecond * Time.deltaTime;

            Character2Ability boost = FindObjectOfType<Character2Ability>();
            if (boost != null)
            {
                damagePerFrame = boost.DealDamage(damagePerFrame);
            }

            enemy.TakeDamage(damagePerFrame);
        }
    }

}
