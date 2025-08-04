using UnityEngine;

public class FaucetButton : MonoBehaviour
{
    public DualButtonTrigger buttonManager;
    private bool hasBeenPressed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenPressed) return;
        if (!other.CompareTag("Player")) return;

        hasBeenPressed = true;
        buttonManager.RegisterButtonPress(this);
    }

    public void ResetButton()
    {
        hasBeenPressed = false;
    }
}