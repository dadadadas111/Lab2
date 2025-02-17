using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletPool bulletPool;

    private void Start()
    {
        bulletPool = FindObjectOfType<BulletPool>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bulletPool.AddToPool(gameObject);
    }

    private void OnBecameInvisible()
    {
        bulletPool.AddToPool(gameObject);
    }
}
