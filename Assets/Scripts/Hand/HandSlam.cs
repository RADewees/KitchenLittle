using UnityEngine;

public class HandSlam : MonoBehaviour
{
    public Vector3 targetPosition;
    public float slamSpeed = 20f;
    public float retreatSpeed = 10f;
    public float slamPauseTime = 0.4f;
    public float damageCooldown = 0.5f;

    private enum State { Falling, Slammed, Retreating }
    private State currentState = State.Falling;

    private float pauseTimer = 0f;
    private float lastDamageTime = -999f;

    void Update()
    {
        switch (currentState)
        {
            case State.Falling:
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, slamSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    currentState = State.Slammed;
                    pauseTimer = slamPauseTime;
                }
                break;

            case State.Slammed:
                pauseTimer -= Time.deltaTime;
                if (pauseTimer <= 0f)
                {
                    currentState = State.Retreating;
                }
                break;

            case State.Retreating:
                transform.position += Vector3.up * retreatSpeed * Time.deltaTime;
                if (transform.position.y > 12f)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (currentState != State.Slammed)
            return;

        if (other.CompareTag("Player") && Time.time - lastDamageTime >= damageCooldown)
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.TakeDamage(3);
                lastDamageTime = Time.time;

                // ✅ Squash player vertically
                Vector3 scale = other.transform.localScale;
                scale.y = 0.1f;
                other.transform.localScale = scale;

                // ✅ Move player to ground level (Y = 0)
                Vector3 pos = other.transform.position;
                pos.y = 0f;
                other.transform.position = pos;

                Debug.Log("Player squashed by hand slam!");
            }
        }
    }

}