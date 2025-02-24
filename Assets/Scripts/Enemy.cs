using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 3;
    public GameObject explosionPrefab;
    public AudioSource deathSound;

    private HealthBarFade healthBarFade;
    private GameManager gameManager;
    private float bottomY = -5f; // Adjust based on your scene
    private Vector2 knockbackForce = new Vector2(1f, 0f); // Adjust knockback force
    private EnemyPool enemyPool;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private DropItemPool dropItemPool;
    private float horizontalRange = 2f; // How far left and right they can move
    private float horizontalSpeed = 1f; // Speed of horizontal movement
    private float randomOffset; // Unique offset for each enemy

    private void Start()
    {
        randomOffset = Random.Range(0f, 100f);
        enemyPool = FindObjectOfType<EnemyPool>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        // find the health bar fade object
        healthBarFade = FindObjectOfType<HealthBarFade>();

        // find the game manager object
        gameManager = FindObjectOfType<GameManager>();

        // find the drop item pool object
        dropItemPool = FindObjectOfType<DropItemPool>();
    }

    private void Update()
    {
        // Calculate horizontal movement using Perlin Noise or sine wave
        float horizontalMovement = Mathf.Sin((Time.time + randomOffset) * horizontalSpeed) * horizontalRange;

        // Move enemy downward with horizontal movement
        transform.position += new Vector3(horizontalMovement * Time.deltaTime, -speed * Time.deltaTime, 0);


        // Return to pool if it reaches the bottom
        if (transform.position.y <= bottomY)
        {
            if (healthBarFade != null)
            {
                healthBarFade.TakeDamage(5);
            }
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
            if (deathSound != null)
            {
                deathSound.Play();
                Debug.Log("Death Sound Played");
            }
            if (GameManager.Instance != null)
            {
                GameManager.Instance.IncreaseKillCount();
            }
            Explode();

            // random chance to drop item
            if (Random.Range(0, 100) < 30)
            {
                DropItem();
            }

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
            explosion.transform.SetParent(gameManager.transform);
            Destroy(explosion, 0.5f); // Destroy after 1 second
        }
    }

    private void DropItem()
    {
        if (dropItemPool != null)
        {
            dropItemPool.GetDropItem(transform.position);
        }
    }

}
