using System.Collections;
using UnityEngine;

public class HandSlamSpawner : MonoBehaviour
{
    [Header("Hand Slam Settings")]
    public GameObject handPrefab;           // The hand to spawn
    public GameObject warningPrefab;        // Warning marker on the table
    public float spawnRadius = 5f;          // Radius around the player
    public float timeBetweenSlams = 3f;     // How often a hand spawns

    [Header("Player Reference")]
    public Transform player;                // Player target

    [Header("Audio")]
    public AudioClip slamClip;              // Sound to play when hand spawns

    private float timer;

    void Update()
    {
        if (player == null) return;

        timer += Time.deltaTime;

        if (timer >= timeBetweenSlams)
        {
            timer = 0f;
            SpawnSlam();
        }
    }

    void SpawnSlam()
    {
        // Choose a random offset near the player
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0f,
            Random.Range(-spawnRadius, spawnRadius)
        );

        Vector3 targetPos = player.position + randomOffset;
        targetPos.y = 0f; // Align to table surface

        // Spawn the warning marker
        GameObject warning = Instantiate(warningPrefab, targetPos, Quaternion.identity);

        // Start coroutine to spawn hand after delay
        StartCoroutine(SpawnHandAfterDelay(targetPos, warning));
    }

    IEnumerator SpawnHandAfterDelay(Vector3 targetPos, GameObject warning)
    {
        yield return new WaitForSeconds(1.2f); // Warning duration

        Vector3 handSpawnPos = targetPos + Vector3.up * 10f;
        GameObject hand = Instantiate(handPrefab, handSpawnPos, Quaternion.identity);

        HandSlam slam = hand.GetComponent<HandSlam>();
        slam.targetPosition = targetPos;

        // Play slam spawn sound
        if (slamClip != null)
            AudioSource.PlayClipAtPoint(slamClip, handSpawnPos);

        Destroy(warning);
    }
}
