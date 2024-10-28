using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
void Update()
{
    // Mengikuti posisi pemain pada sumbu x tanpa batasan rentang
    if (player.position.x != transform.position.x)
    {
        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(player.position.x, transform.position.y, transform.position.z),
            0.1f
        );
    }
}

}
