using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Key Collection")]
    public int totalKeyPieces = 3;
    private int collectedKeys = 0;

    public TextMeshProUGUI keyPieceText;
    public GameObject winScreenUI;   // Set active when you win
    public AudioSource winSound;     // Play on victory
    public AudioSource pickupSound;


    private bool hasWon = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateKeyUI();
        if (winScreenUI != null)
            winScreenUI.SetActive(false);
    }

    public void CollectKeyPiece()
    {
        if (pickupSound != null)
            pickupSound.Play();

        if (hasWon) return;

        collectedKeys++;
        UpdateKeyUI();

        if (pickupSound != null)
            pickupSound.Play();

        if (collectedKeys >= totalKeyPieces)
        {
            HandleWin();
        }
    }


    void UpdateKeyUI()
    {
        if (keyPieceText != null)
            keyPieceText.text = $"Key Pieces: {collectedKeys} / {totalKeyPieces}";
    }

    void HandleWin()
    {
        winScreenUI.SetActive(false); // in Start()

        hasWon = true;
        Debug.Log("You collected all the key pieces!");

        if (winSound != null)
            winSound.Play();

        if (winScreenUI != null)
            winScreenUI.SetActive(true);

        // Optional: freeze game
        Time.timeScale = 0f;
    }
}
