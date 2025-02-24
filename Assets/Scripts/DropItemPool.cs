using UnityEngine;
using System.Collections.Generic;

public class DropItemPool : MonoBehaviour
{
    public GameObject dropItemPrefab;
    public int poolSize = 10;
    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject item = Instantiate(dropItemPrefab);
            item.transform.SetParent(transform);
            item.SetActive(false);
            pool.Enqueue(item);
        }
    }

    public GameObject GetDropItem(Vector3 position)
    {
        if (pool.Count == 0)
        {
            GrowPool(5);
        }

        GameObject item = pool.Dequeue();
        item.transform.position = position;
        item.GetComponent<DropItem>().buffType = Random.Range(0, 2) == 0 ? DropItem.BuffType.FireRate : DropItem.BuffType.Health;
        item.SetActive(true);

        return item;
    }

    public void ReturnToPool(GameObject item)
    {
        item.SetActive(false);
        pool.Enqueue(item);
    }

    private void GrowPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject item = Instantiate(dropItemPrefab);
            item.SetActive(false);
            pool.Enqueue(item);
        }
    }
}
