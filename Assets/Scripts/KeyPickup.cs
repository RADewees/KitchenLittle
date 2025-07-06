using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectKeyPiece();
            Destroy(gameObject);
        }
    }
}