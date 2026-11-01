using UnityEngine;
using UnityEngine.InputSystem;

public class Catcher : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public float limit = 7.5f;

    void Start()
    {
        // Make sure this GameObject has the "Catcher" tag
        // so Coin.cs can detect the collision
        gameObject.tag = "Catcher";
    }

    void Update()
    {
        // Don't allow movement when game is over
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
