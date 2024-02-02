using UnityEngine;

public class AimController : MonoBehaviour
{
    public Transform playerTransform;
    public float rotationSpeed = 5f;
    public LayerMask enemyLayer;

    private Transform targetEnemy;

    void Update()
    {
        // Check for player input or any other mechanism to lock onto enemies
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Find the nearest enemy and lock onto it
            LockOntoNearestEnemy();
        }

        // Rotate towards the locked enemy (if any)
        if (targetEnemy != null)
        {
            RotateTowardsEnemy();
        }
    }

    void LockOntoNearestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(playerTransform.position, 10f, enemyLayer);

        if (enemies.Length > 0)
        {
            float closestDistance = Mathf.Infinity;
            Transform nearestEnemy = null;

            foreach (Collider enemyCollider in enemies)
            {
                float distanceToEnemy = Vector3.Distance(playerTransform.position, enemyCollider.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    nearestEnemy = enemyCollider.transform;
                }
            }

            targetEnemy = nearestEnemy;
        }
        else
        {
            // No enemies in range, reset target
            targetEnemy = null;
        }
    }

    void RotateTowardsEnemy()
    {
        Vector3 directionToEnemy = (targetEnemy.position - playerTransform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}