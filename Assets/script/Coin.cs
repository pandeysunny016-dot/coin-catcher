using UnityEngine;

public class Coin : MonoBehaviour
{
    public float fallSpeed  = 4f;
    public float destroyY   = -6f;
    public int   scoreValue = 10;

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Missed — fell off screen
        if (transform.position.y <= destroyY)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.LoseLife();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Caught by the catcher
        if (other.CompareTag("Catcher"))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}
