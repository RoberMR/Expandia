using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Joystick")]
    private VirtualJoystick joystick;

    private void Awake()
    {
        joystick = FindObjectOfType<VirtualJoystick>();

        if (joystick == null)
            Debug.LogError("VirtualJoystick not found in scene!");
    }

    private void Update()
    {
        if (joystick == null) return;

        Vector2 input = joystick.InputVector;
        Vector3 movement = new Vector3(input.x, 0f, input.y);

        transform.position += movement * moveSpeed * Time.deltaTime;

        if (movement.sqrMagnitude > 0.001f)
            transform.forward = movement;
    }
}