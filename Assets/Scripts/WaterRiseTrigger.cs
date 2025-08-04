using System.Collections;
using UnityEngine;

public class WaterRiseTrigger : MonoBehaviour
{
    public Transform waterParent; // Parent of water & floating objects
    public float riseAmount = 5f; // Amount to rise RELATIVE to current height
    public float riseSpeed = 1f;

    public AudioSource faucetSound;
    public ParticleSystem faucetParticles;

    private bool hasTriggered = false;

    public void TriggerWater()
    {
        if (hasTriggered) return;

        hasTriggered = true;

        if (faucetSound != null) faucetSound.Play();
        if (faucetParticles != null) faucetParticles.Play();

        StartCoroutine(RaiseWater());
    }

    private IEnumerator RaiseWater()
    {
        Vector3 startPos = waterParent.position;
        Vector3 targetPos = startPos + new Vector3(0f, riseAmount, 0f);  // use offset

        while (Vector3.Distance(waterParent.position, targetPos) > 0.01f)
        {
            waterParent.position = Vector3.MoveTowards(waterParent.position, targetPos, riseSpeed * Time.deltaTime);
            yield return null;
        }
    }
}