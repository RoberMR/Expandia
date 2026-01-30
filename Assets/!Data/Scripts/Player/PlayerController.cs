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

// MOVEMENT WITH RIGIDBODY
/*
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] VirtualJoystick joystick;

    Rigidbody rb;
    Vector3 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector2 input = joystick.InputVector;
        movement = new Vector3(input.x, 0f, input.y);
    }

    void FixedUpdate()
    {
        if (movement.sqrMagnitude < 0.001f)
            return;

        Vector3 targetPosition =
            rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(targetPosition);

        rb.MoveRotation(Quaternion.LookRotation(movement));
    }
}
*/