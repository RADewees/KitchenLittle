using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualButtonTrigger : MonoBehaviour
{
    [Tooltip("Time allowed between first and second button presses")]
    public float buttonTimeLimit = 20f;

    [Tooltip("Reference to the water rising logic")]
    public WaterRiseTrigger waterRiseTrigger;

    [Tooltip("Countdown sound after first press")]
    public AudioSource countdownSound;

    private FaucetButton firstPressedButton;
    private bool isTriggered = false;
    private Coroutine countdownCoroutine;

    public void RegisterButtonPress(FaucetButton button)
    {
        if (isTriggered) return;

        if (firstPressedButton == null)
        {
            firstPressedButton = button;
            Debug.Log("First button pressed!");
            countdownCoroutine = StartCoroutine(WaitForSecondButton());
            if (countdownSound != null) countdownSound.Play();
        }
        else if (button != firstPressedButton)
        {
            Debug.Log("Second button pressed! Triggering water.");
            isTriggered = true;
            if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);
            waterRiseTrigger.TriggerWater();
        }
    }

    private IEnumerator WaitForSecondButton()
    {
        yield return new WaitForSeconds(buttonTimeLimit);

        Debug.Log("Second button not pressed in time. Resetting.");
        firstPressedButton.ResetButton();
        firstPressedButton = null;
    }
}