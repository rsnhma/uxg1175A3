using UnityEngine;

public class EnemyPlayerAwareness : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; }
    public Vector2 DirectionToPlayer { get; private set; }

   private float playerAwarenessDistance = 3f;

    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Vector2 toPlayer = player.position - transform.position;
        DirectionToPlayer = toPlayer.normalized;

        AwareOfPlayer = toPlayer.magnitude <= playerAwarenessDistance;
    }
}
