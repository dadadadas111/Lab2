using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int initialPoolSize = 10;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    private void Start()
    {
        GrowPool(initialPoolSize);
    }

    public void GrowPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    public GameObject GetFromPool()
    {
        if (bulletPool.Count == 0)
        {
            GrowPool(5); // Expand if empty
        }

        GameObject bullet = bulletPool.Dequeue();
        bullet.SetActive(true);
        return bullet;
    }

    public void AddToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
