using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private AudioSource audioHurt;

    private AudioSource audioDeath;
    
    private AudioSource audioJump;
    private AudioSource audioWalk;
    private AudioSource audioBGM;

    private bool isDead = false;
    private AudioSource audioSource;

    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float jumpForce = 8f;
    public float gravity = -20f;

    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;
    public PlayerHealthUI healthUI; // ← drag HealthHUD here in Inspector

    private CharacterController controller;
    private Transform cam;

    private Vector2 moveInput;
    private bool jumpPressed;

    private Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        
        // Assign Audio Sources
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length >= 4)
        {
            audioJump  = sources[0];
            audioWalk  = sources[1];
            audioBGM   = sources[2];
            audioDeath = sources[3];
            audioBGM.Play();
        }



        if (healthUI != null)
            healthUI.SetHealth(currentHealth);
        else
            Debug.LogWarning("Health UI not assigned to PlayerController.");
    }

    void Update()
    {
        if (!isDead)
        {
            ApplyGravityAndJump();
            HandleMovement();
        }

        // TEMP: Press 'K' to simulate taking damage
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            TakeDamage(1);
        }
    }



    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        jumpPressed = value.isPressed;
    }

    void ApplyGravityAndJump()
    {
        if (controller.isGrounded)
        {
            if (velocity.y < 0f)
                velocity.y = -2f;

            if (jumpPressed)
            {
                velocity.y = jumpForce;
                jumpPressed = false;
                Debug.Log("Jump triggered! velocity.y = " + velocity.y);

                if (audioJump != null)
                    audioJump.Play();
            }
        }

        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
    }

    void HandleMovement()
    {
        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y);

        if (inputDir.sqrMagnitude < 0.01f)
        {
            controller.Move(velocity * Time.deltaTime);
            return;
        }

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;
        moveDir.Normalize();

        Vector3 finalMove = moveDir * moveSpeed;
        finalMove.y = velocity.y;

        controller.Move(finalMove * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        
        bool isMoving = moveDir.sqrMagnitude > 0.01f;
        bool isGrounded = controller.isGrounded;

        if (isGrounded && isMoving)
        {
            if (!audioWalk.isPlaying)
                audioWalk.Play();
        }
        else
        {
            if (audioWalk.isPlaying)
                audioWalk.Stop();
        }

        
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        if (healthUI != null)
            healthUI.SetHealth(currentHealth);

        if (audioHurt != null)
            audioHurt.Play();

        Debug.Log("Player took damage. Current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    void HandleDeath()
    {
        isDead = true;
        Debug.Log("Player died. Restarting scene in 5 seconds...");

        if (audioDeath != null)
            audioDeath.Play();

        StartCoroutine(RestartAfterDelay(5f));
    }


    IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
