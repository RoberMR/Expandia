using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, -10f);
    [SerializeField] private float followSpeed = 10f;

    [Header("Target")]
    private Transform target;

    private void Awake()
    {
        PlayerController player = FindObjectOfType<PlayerController>();

        if (player != null)
            target = player.transform;
        else
            Debug.LogError("PlayerController not found in scene!");
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );
    }
}