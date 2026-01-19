using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VirtualJoystick joystick;

    [Header("Player values")]
    [SerializeField] private float moveSpeed = 5f;

    private void Update()
    {
        Vector2 input = joystick.InputVector;
        Vector3 movement = new Vector3(input.x, 0f, input.y);

        transform.position += movement * moveSpeed * Time.deltaTime;

        if (movement.sqrMagnitude > 0.001f)
            transform.forward = movement;
    }
}