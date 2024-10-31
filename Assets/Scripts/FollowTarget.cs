using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        // Mengikuti posisi pemain pada sumbu x dan y
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
    }
}
