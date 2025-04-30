using UnityEngine;

public class ZombieFollowPlayer : MonoBehaviour
{
    public Transform player; // Assign the player GameObject in the Inspector
    public float moveSpeed = 3f; // Movement speed of the zombie

    private void Update()
    {
        if (player != null)
        {
            // Move toward the player
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Optionally, rotate to face the player
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}