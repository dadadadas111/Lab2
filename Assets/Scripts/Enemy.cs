using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 3;
    public GameObject explosionPrefab;

    private float bottomY = -5f; // Adjust based on your scene
    private Vector2 knockbackForce = new Vector2(1f, 0f); // Adjust knockback force
    private EnemyPool enemyPool;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        enemyPool = FindObjectOfType<EnemyPool>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void Update()
    {
        // Move enemy downward
        transform.position += Vector3.down * speed * Time.deltaTime;

        // Return to pool if it reaches the bottom
        if (transform.position.y <= bottomY)
        {
            ResetEnemy();
        }

        if (transform.position.x > 20 || transform.position.x < -20 || transform.position.y > 10 || transform.position.y < -10)
        {
            ResetEnemy();
        }
    }


    public void TakeDamage(int damage, Vector2 bulletDirection)
    {
        health -= damage;
        StartCoroutine(FlashRed());

        // Apply knockback force
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(bulletDirection * knockbackForce, ForceMode2D.Impulse);
        }

    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.2f); // Adjust flash duration
            spriteRenderer.color = originalColor;
        }

        // If health is 0 or below, deactivate enemy and return to pool
        if (health <= 0)
        {
            Explode();
            ResetEnemy();
        }
    }

    private void ResetEnemy()
    {
        health = 3; // Reset health
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        enemyPool.AddToPool(gameObject);
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f); // Destroy after 1 second
        }
    }
}
