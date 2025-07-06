using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform target;                      // CameraTarget
    public InputActionReference lookAction;

    [Header("Positioning")]
    public Vector3 offset = new Vector3(0, 3, -6);

    [Header("Sensitivity & Smoothness")]
    public float mouseSensitivity = 2f;
    public float followSmoothTime = 0.1f;
    public float ySmoothTime = 0.2f;              // ← slower Y smooth

    private float yaw;
    private float pitch;

    private Vector3 currentVelocity;              // For X/Z smoothing
    private float yVelocity;                      // ← for Y smoothing

    void OnEnable()  { lookAction.action.Enable(); }
    void OnDisable() { lookAction.action.Disable(); }

    void LateUpdate()
    {
        
        // Mouse look input
        Vector2 look = lookAction.action.ReadValue<Vector2>();
        yaw   += look.x * mouseSensitivity;
        pitch -= look.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -55f, 50f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        // Smooth X and Z with SmoothDamp
        Vector3 smoothed = transform.position;
        smoothed.x = Mathf.SmoothDamp(transform.position.x, desiredPosition.x, ref currentVelocity.x, followSmoothTime);
        smoothed.z = Mathf.SmoothDamp(transform.position.z, desiredPosition.z, ref currentVelocity.z, followSmoothTime);

        // Smooth Y independently with its own time
        smoothed.y = Mathf.SmoothDamp(transform.position.y, desiredPosition.y, ref yVelocity, ySmoothTime);

        // Apply position and rotation
        transform.position = smoothed;
        transform.LookAt(target.position + Vector3.up * 1.0f);
    }
}