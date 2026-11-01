using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Coin Settings")]
    public GameObject coinPrefab;
    public float spawnInterval = 1.5f;
    public float spawnRangeX = 7f;
    public float spawnY = 6f;

    [Header("Difficulty")]
    public float minInterval = 0.4f;       // Fastest spawn rate
    public float difficultyRate = 0.05f;   // How fast interval decreases per second

    private float timer = 0f;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
            return;

        // Gradually increase difficulty over time
        spawnInterval = Mathf.Max(minInterval, spawnInterval - difficultyRate * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCoin();
            timer = 0f;
        }
    }

    void SpawnCoin()
    {
        if (coinPrefab == null) return;

        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);
        Instantiate(coinPrefab, spawnPos, Quaternion.identity);
    }
}
