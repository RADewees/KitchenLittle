using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 10f;
    public float chaseRadius = 15f;
    public float damageCooldown = 0.5f;

    private Transform player;
    private NavMeshAgent agent;
    private Vector3 startPosition;

    private float lastDamageTime = -999f; // Time when damage was last applied

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float distanceToHome = Vector3.Distance(player.position, startPosition);

        if (distanceToPlayer <= detectionRange && distanceToHome <= chaseRadius)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.SetDestination(startPosition);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check cooldown before applying damage
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                PlayerController playerScript = other.GetComponent<PlayerController>();
                if (playerScript != null)
                {
                    playerScript.TakeDamage(1);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}