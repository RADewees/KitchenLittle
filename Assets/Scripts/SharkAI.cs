using UnityEngine;
using UnityEngine.AI;

public class SharkAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float damageCooldown = 0.5f;

    private NavMeshAgent agent;
    private bool canDamage = true;
    private bool playerInWater = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null || !playerInWater)
        {
            agent.ResetPath();
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
        }
    }

    // Called externally when player enters water
    public void SetPlayerInWater(bool inWater)
    {
        playerInWater = inWater;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canDamage)
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.TakeDamage(1);
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private System.Collections.IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}
