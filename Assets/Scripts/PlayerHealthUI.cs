using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Image[] hearts;            // Assign in Inspector
    public Sprite fullHeart;          // Full heart icon
    public Sprite emptyHeart;         // Empty heart icon

    public void SetHealth(int currentHealth)
    {
        Debug.Log("Updating UI to " + currentHealth + " HP");
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }
    
}
