using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    public float fallSpeed = 4f;
    public float destroyY = -6f;      // Y position where coin is considered missed
    public int scoreValue = 10;

    void Update()
    {
        // Move coin downward
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Coin missed — fell off screen
        if (transform.position.y <= destroyY)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.LoseLife();

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Catcher"))
        {
            // Coin caught!
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(scoreValue);

            Destroy(gameObject);
        }
    }
}
