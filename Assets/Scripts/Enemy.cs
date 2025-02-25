using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 3;
    public GameObject explosionPrefab;
    public AudioSource deathSound;
    public GameObject hpBarPrefab;

    private GameObject hpBarInstance;
    private Image hpFillImage;
    private HealthBarFade healthBarFade;
    private GameManager gameManager;
    private float bottomY = -5f;
    private Vector2 knockbackForce = new Vector2(1f, 0f);
    private EnemyPool enemyPool;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private DropItemPool dropItemPool;
    private float horizontalRange = 2f;
    private float horizontalSpeed = 1f;
    private float randomOffset;

    private void Start()
    {
        randomOffset = UnityEngine.Random.Range(0f, 100f);
        enemyPool = FindObjectOfType<EnemyPool>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        if (hpBarPrefab != null)
        {
            hpBarInstance = Instantiate(hpBarPrefab, transform);
            hpBarInstance.transform.SetParent(GameObject.Find("Canvas").transform);
            hpBarInstance.transform.SetAsFirstSibling();
            hpBarInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            hpFillImage = hpBarInstance.transform.Find("Bar").GetComponent<Image>();
            hpBarInstance.transform.localPosition = new Vector3(0, 0.6f, 0);
            hpBarInstance.SetActive(false);
        }

        healthBarFade = FindObjectOfType<HealthBarFade>();
        gameManager = FindObjectOfType<GameManager>();
        dropItemPool = FindObjectOfType<DropItemPool>();
    }

    private void Update()
    {
        float horizontalMovement = Mathf.Sin((Time.time + randomOffset) * horizontalSpeed) * horizontalRange;
        transform.position += new Vector3(horizontalMovement * Time.deltaTime, -speed * Time.deltaTime, 0);
        // Clamp within boundaries
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -8f, 8f),
            transform.position.y,
            transform.position.z
        );

        // check to flip sprite based on horizontal movement
        if (horizontalMovement < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalMovement > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (hpBarInstance != null)
        {
            hpBarInstance.transform.position = transform.position + new Vector3(0, 0.5f, 0);
        }
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
        UpdateHealthBar();
        StartCoroutine(FlashRed());
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
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = originalColor;
        }
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
            if (UnityEngine.Random.Range(0, 100) < 50)
            {
                DropItem();
            }

            ResetEnemy();
        }
    }

    private void ResetEnemy()
    {
        health = 3;
        UpdateHealthBar();
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
            Destroy(explosion, 0.5f);
        }
    }

    private void DropItem()
    {
        if (dropItemPool != null)
        {
            dropItemPool.GetDropItem(transform.position);
        }
    }

    private void UpdateHealthBar()
    {
        if (health == 3)
        {
            if (hpBarInstance != null)
            {
                hpBarInstance.SetActive(false);
            }
        }
        else
        {
            if (hpBarInstance != null)
            {
                hpBarInstance.SetActive(true);
            }
        }

        if (hpFillImage != null)
        {
            hpFillImage.fillAmount = (float)health / 3;
        }
    }

}
