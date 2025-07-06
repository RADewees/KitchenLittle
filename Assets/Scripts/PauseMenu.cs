using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioSource musicSource;      // Reference to BGM AudioSource
    public AudioSource[] sfxSources;     // Reference to all SFX AudioSources

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

        // Load volume settings
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        ApplyVolumes();
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Debug.Log("P key pressed — toggling pause.");
            TogglePause();
        }
    }

    
    public void OnPause(InputValue value)
    {
        if (value.isPressed)
        {
            TogglePause();
        }
    }


    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
            if (musicSource != null)
                musicSource.Pause();
        }
        else
        {
            Time.timeScale = 1f;
            if (musicSource != null)
                musicSource.Play();
        }
    }

    public void OnMusicVolumeChanged(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        if (musicSource != null)
            musicSource.volume = volume;
    }

    public void OnSFXVolumeChanged(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        foreach (var src in sfxSources)
        {
            if (src != null)
                src.volume = volume;
        }
    }

    private void ApplyVolumes()
    {
        OnMusicVolumeChanged(musicSlider.value);
        OnSFXVolumeChanged(sfxSlider.value);
    }
}