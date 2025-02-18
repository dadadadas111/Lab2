using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletPool bulletPool;
    public int damage = 1;
    private void Start()
    {
        bulletPool = FindObjectOfType<BulletPool>();
    }

    private void Update()
    {
        // if the transform x or y is greater than 50 or less than -50, return to pool
        if (transform.position.x > 20 || transform.position.x < -20 || transform.position.y > 10 || transform.position.y < -10)
        {
            bulletPool.AddToPool(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the enemy and apply damage and knockback
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                //Debug.Log("Bullet hit enemy");
                Vector2 bulletDirection = (transform.position - collision.transform.position).normalized;
                enemy.TakeDamage(damage, bulletDirection); // Apply damage and knockback
                bulletPool.AddToPool(gameObject);
            }
        }
    }

    //private void OnBecameInvisible()
    //{
    //    bulletPool.AddToPool(gameObject);
    //}
}
