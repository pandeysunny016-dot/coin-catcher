using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Game State
    [HideInInspector] public int score = 0;
    [HideInInspector] public int lives = 3;
    [HideInInspector] public bool isGameOver = false;

    // Auto-created UI
    private Text scoreText;
    private Text livesText;
    private Text finalScoreText;
    private GameObject gameOverPanel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        BuildUI();
        UpdateUI();
    }

    // ── Creates all UI automatically at runtime ──────────────────────────────
    void BuildUI()
    {
        // Canvas
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }

        // Score label
        scoreText = CreateLabel(canvas.transform, "ScoreText", "Score: 0",
            new Vector2(10, -10), new Vector2(200, 40), new Vector2(0, 1), new Vector2(0, 1));
        scoreText.alignment = TextAnchor.UpperLeft;

        // Lives label
        livesText = CreateLabel(canvas.transform, "LivesText", "Lives: 3",
            new Vector2(-10, -10), new Vector2(200, 40), new Vector2(1, 1), new Vector2(1, 1));
        livesText.alignment = TextAnchor.UpperRight;

        // Game Over Panel (hidden by default)
        gameOverPanel = new GameObject("GameOverPanel");
        gameOverPanel.transform.SetParent(canvas.transform, false);
        Image panelImg = gameOverPanel.AddComponent<Image>();
        panelImg.color = new Color(0, 0, 0, 0.75f);
        RectTransform panelRect = gameOverPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.2f, 0.3f);
        panelRect.anchorMax = new Vector2(0.8f, 0.7f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        // "GAME OVER" title
        CreateLabel(gameOverPanel.transform, "GameOverTitle", "GAME OVER",
            new Vector2(0, 60), new Vector2(300, 60), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            32, Color.red);

        // Final score text
        finalScoreText = CreateLabel(gameOverPanel.transform, "FinalScore", "Final Score: 0",
            new Vector2(0, 0), new Vector2(300, 40), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            22, Color.white);

        // Restart button
        GameObject btnGO = new GameObject("RestartButton");
        btnGO.transform.SetParent(gameOverPanel.transform, false);
        Image btnImg = btnGO.AddComponent<Image>();
        btnImg.color = new Color(0.2f, 0.7f, 0.2f);
        Button btn = btnGO.AddComponent<Button>();
        btn.onClick.AddListener(RestartGame);
        RectTransform btnRect = btnGO.GetComponent<RectTransform>();
        btnRect.anchoredPosition = new Vector2(0, -55);
        btnRect.sizeDelta = new Vector2(160, 45);
        btnRect.anchorMin = new Vector2(0.5f, 0.5f);
        btnRect.anchorMax = new Vector2(0.5f, 0.5f);
        CreateLabel(btnGO.transform, "BtnLabel", "RESTART",
            Vector2.zero, new Vector2(160, 45), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            20, Color.white);

        gameOverPanel.SetActive(false);
    }

    // Helper: create a UI Text element
    Text CreateLabel(Transform parent, string name, string text,
        Vector2 anchoredPos, Vector2 size,
        Vector2 anchorMin, Vector2 anchorMax,
        int fontSize = 24, Color? color = null)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        Text t = go.AddComponent<Text>();
        t.text = text;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize = fontSize;
        t.color = color ?? Color.white;
        t.fontStyle = FontStyle.Bold;
        t.alignment = TextAnchor.MiddleCenter;
        RectTransform r = go.GetComponent<RectTransform>();
        r.anchorMin = anchorMin;
        r.anchorMax = anchorMax;
        r.anchoredPosition = anchoredPos;
        r.sizeDelta = size;
        return t;
    }

    // ── Public methods called by other scripts ────────────────────────────────
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
        if (lives <= 0) TriggerGameOver();
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        CoinSpawner spawner = FindFirstObjectByType<CoinSpawner>();
        if (spawner != null) spawner.enabled = false;

        foreach (GameObject coin in GameObject.FindObjectsByType<Coin>(FindObjectsSortMode.None))
            Destroy(coin.gameObject);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (finalScoreText != null)
                finalScoreText.text = "Final Score: " + score;
        }
    }

    public void RestartGame()
    {
        Destroy(gameObject); // destroy this so a fresh one is created
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (livesText != null) livesText.text = "Lives: " + lives;
    }
}
