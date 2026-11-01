using UnityEngine;
using UnityEngine.InputSystem;

public class Catcher : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public float limit = 7.5f;

    void Awake()
    {
        // Auto-add a BoxCollider2D if missing
        if (GetComponent<BoxCollider2D>() == null)
            gameObject.AddComponent<BoxCollider2D>();

        // Auto-create GameManager if missing
        if (GameManager.Instance == null)
        {
            GameObject gm = new GameObject("GameManager");
            gm.AddComponent<GameManager>();
        }

        // Auto-create CoinSpawner if missing
        if (FindFirstObjectByType<CoinSpawner>() == null)
        {
            GameObject spawner = new GameObject("CoinSpawner");
            spawner.AddComponent<CoinSpawner>();
        }
    }

    void Start()
    {
        // No tag needed — Coin.cs detects collision via GetComponent<Catcher>()
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
            return;

        float input = 0f;
        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            input = -1f;
        else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            input = 1f;

        transform.Translate(speed * input * Vector3.right * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -limit, limit);
        transform.position = pos;
    }
}
