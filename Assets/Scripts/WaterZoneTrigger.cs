using UnityEngine;

public class WaterZoneTrigger : MonoBehaviour
{
    private SharkAI shark;

    void Start()
    {
        shark = FindObjectOfType<SharkAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && shark != null)
        {
            shark.SetPlayerInWater(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && shark != null)
        {
            shark.SetPlayerInWater(false);
        }
    }
}