using UnityEngine;

public class AutoAim : MonoBehaviour
{
    public string enemyTag = "Enemy"; // Tag assigned to enemy GameObjects
    public float aimTurnSpeed = 5f;    // Speed of turn response when aiming

    private Transform playerTransform;

    void Start()
    {
        playerTransform = transform;
    }

    void Update()
    {
        // Find the nearest enemy
        Transform nearestEnemy = FindNearestEnemy();

        // If there is a nearest enemy, aim at its head
        if (nearestEnemy != null)
        {
            AimAtHead(nearestEnemy);
        }
    }

    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(playerTransform.position, enemy.transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }

    void AimAtHead(Transform target)
    {
        // Calculate direction to the target's head
        Vector3 direction = target.position - playerTransform.position;
        direction.y = 0f; // Ignore height difference

        // Rotate towards the target's head
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, lookRotation, aimTurnSpeed * Time.deltaTime);
    }
}