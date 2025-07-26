using UnityEngine;

public class EnemyPlayerAwareness : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; }
    public Vector2 DirectionToPlayer { get; private set; }

    private float playerAwarenessDistance = 3f;
    private Transform player;

    private void Update()
    {
        // Retry finding player until successful
        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null)
                player = found.transform;
            return; // skip awareness check this frame
        }

        Vector2 toPlayer = player.position - transform.position;
        DirectionToPlayer = toPlayer.normalized;

        AwareOfPlayer = toPlayer.magnitude <= playerAwarenessDistance;

        if (AwareOfPlayer)
        {
            Debug.DrawLine(transform.position, player.position, Color.red);
        }
        else
        {
            Debug.DrawLine(transform.position, player.position, Color.gray);
        }


    }
}
