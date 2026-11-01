using UnityEngine;
using UnityEngine.InputSystem;

public class Catcher : MonoBehaviour
{
    public float speed= 10f;
    public float limit = 7.5f;

    void Start()
    {
        
    }

    void Update()
    {
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
