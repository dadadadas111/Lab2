using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPool enemyPool;
    public float spawnRate = 1.5f;
    public float spawnXRange = 8f;
    public float spawnY = 6f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnRate);
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnXRange, spawnXRange), spawnY, 0);
        enemyPool.GetFromPool(spawnPosition);
    }
}
