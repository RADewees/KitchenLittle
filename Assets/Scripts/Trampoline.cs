using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("Trampoline Settings")]
    public float bounceForce = 20f;
    public AudioSource bounceSound;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player hit it from above
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null && controller.isGrounded)
            {
                player.Bounce(bounceForce);

                if (bounceSound != null)
                    bounceSound.Play();
            }
        }
    }
}
