using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public int score = 0;
    public int lives = 3;
    public bool isGameOver = false;

    [Header("UI References")]
    public Text scoreText;
    public Text livesText;
    public GameObject gameOverPanel;
    public Text finalScoreText;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateUI();
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;
        score += amount;
        UpdateUI();
    }

    public void LoseLife()
    {
        if (isGameOver) return;
        lives--;
        UpdateUI();

        if (lives <= 0)
            TriggerGameOver();
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        // Stop coin spawning
        CoinSpawner spawner = FindFirstObjectByType<CoinSpawner>();
        if (spawner != null)
            spawner.enabled = false;

        // Destroy all existing coins
        foreach (GameObject coin in GameObject.FindGameObjectsWithTag("Coin"))
            Destroy(coin);

        // Show game over UI
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (finalScoreText != null)
                finalScoreText.text = "Final Score: " + score;
        }

        Debug.Log("Game Over! Final Score: " + score);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }
}
