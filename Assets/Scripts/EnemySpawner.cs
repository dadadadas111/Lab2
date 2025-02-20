using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPool enemyPool;
    public float initialSpawnRate = 1.5f;
    public float minSpawnRate = 0.5f; // The fastest it can get
    public float spawnAcceleration = 0.1f; // How much to decrease per step
    public float spawnXRange = 8f;
    public float spawnY = 6f;
    public float startReducingAfter = 30f; // Time to wait before starting to reduce spawn rate

    private float currentSpawnRate;
    private float currentSpeed = 2f;
    private float timer;
    private bool isPaused = false;

    private void Start()
    {
        currentSpawnRate = initialSpawnRate;
        timer = 0f;
        isPaused = false;
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (!isPaused)
        {
            if (isPaused) yield break;
            SpawnEnemy();
            yield return new WaitForSeconds(currentSpawnRate);

            // Increase the timer
            timer += currentSpawnRate;

            // Start reducing the spawn rate after the specified time
            if (timer >= startReducingAfter && currentSpawnRate > minSpawnRate)
            {
                currentSpawnRate -= spawnAcceleration;
            }

            // Increase the speed of the enemies
            if (timer >= startReducingAfter && currentSpeed < 3.3f)
            {
                currentSpeed += 0.1f;
            }
        }
    }

    private void SpawnEnemy()
    {
        if (isPaused) return;
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnXRange, spawnXRange), spawnY, 0);
        var enemy = enemyPool.GetFromPool(spawnPosition);
        enemy.GetComponent<Enemy>().speed = currentSpeed;
    }
}
