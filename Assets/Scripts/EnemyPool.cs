using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public GameObject enemyPrefab;  // Prefab for enemies
    public int initialPoolSize = 10; // Initial size of the pool

    private Queue<GameObject> enemyPool = new Queue<GameObject>();  // Queue for managing enemies

    private void Start()
    {
        GrowPool(initialPoolSize);  // Initialize pool at the start
    }

    // Grow the pool by a specific amount
    public void GrowPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);  // Initially deactivate the enemy
            Collider2D collider = enemy.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;  // Disable collider when inactive
            }
            enemyPool.Enqueue(enemy);  // Add to the pool
        }
    }

    // Get an inactive enemy from the pool
    public GameObject GetFromPool(Vector3 spawnPosition)
    {
        if (enemyPool.Count == 0)
        {
            GrowPool(10);  // Expand pool if empty
        }

        // Get the first inactive enemy from the pool
        GameObject enemy = enemyPool.Dequeue();
        enemy.transform.position = spawnPosition;  // Set the spawn position
        enemy.SetActive(true);  // Activate the enemy
        Collider2D collider = enemy.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;  // Enable collider when active
        }
        return enemy;
    }

    // Add an enemy back into the pool
    public void AddToPool(GameObject enemy)
    {
        enemy.SetActive(false);  // Deactivate the enemy
        enemy.transform.position = new Vector3(0, -15, 0);  // Reset position
        Collider2D collider = enemy.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;  // Disable collider when inactive
        }
        enemyPool.Enqueue(enemy);  // Return to the pool
    }
}
