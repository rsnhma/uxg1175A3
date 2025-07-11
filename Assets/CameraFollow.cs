using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // set this to your player
    public float smoothSpeed = 5f;
    public Vector3 offset;   // e.g. (0, 0, -10) for 2D camera

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
