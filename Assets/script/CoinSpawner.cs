using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public float spawnInterval = 1.5f;
    public float spawnRangeX  = 7f;
    public float spawnY       = 6f;

    [Header("Difficulty")]
    public float minInterval    = 0.4f;
    public float difficultyRate = 0.02f;   // seconds removed from interval per second

    private float timer = 0f;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        // Gradually speed up spawning
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
        float x = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 pos = new Vector3(x, spawnY, 0f);

        // Build the coin entirely in code — no prefab needed
        GameObject coin = new GameObject("Coin");
        coin.tag = "Coin";
        coin.transform.position = pos;

        // Yellow circle sprite
        SpriteRenderer sr = coin.AddComponent<SpriteRenderer>();
        sr.sprite = CreateCircleSprite();
        sr.color  = Color.yellow;

        // Physics: trigger collider (no gravity — we move it ourselves)
        CircleCollider2D col = coin.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius    = 0.4f;

        // Coin behaviour
        coin.AddComponent<Coin>();
    }

    // Generates a simple filled-circle sprite at runtime
    Sprite CreateCircleSprite()
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size);
        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size / 2f - 2f;

        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), center);
                tex.SetPixel(x, y, dist <= radius ? Color.white : Color.clear);
            }
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 64f);
    }
}
