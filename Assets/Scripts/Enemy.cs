using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 3;
    private float bottomY = -5f; // Adjust based on your scene
    private Vector2 knockbackForce = new Vector2(1f, 0f); // Adjust knockback force
    private EnemyPool enemyPool;

    private void Start()
    {
        enemyPool = FindObjectOfType<EnemyPool>();
    }

    private void Update()
    {
        // Move enemy downward
        transform.position += Vector3.down * speed * Time.deltaTime;

        // Return to pool if it reaches the bottom
        if (transform.position.y <= bottomY)
        {
            health = 3; // Reset health
            enemyPool.AddToPool(gameObject); // Return to pool
        }
    }

    public void TakeDamage(int damage, Vector2 bulletDirection)
    {
        // Apply damage to the enemy
        health -= damage;

        // Apply knockback force
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Reset previous movement
            rb.AddForce(bulletDirection * knockbackForce, ForceMode2D.Impulse); // Apply knockback
        }

        // If health is 0 or below, deactivate enemy and return to pool
        if (health <= 0)
        {
            health = 3; // Reset health
            enemyPool.AddToPool(gameObject);  // Return to pool
        }
    }
}
