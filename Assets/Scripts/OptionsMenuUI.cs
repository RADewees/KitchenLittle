using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenuUI : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Toggles")]
    public Toggle vsyncToggle;
    public Toggle fullscreenToggle;

    [Header("Dropdowns")]
    public TMP_Dropdown resolutionDropdown;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;

    [Header("Audio")]
    public AudioMixer audioMixer; // Set up exposed parameters
    public AudioSource clickSound;

    private Resolution[] resolutions;
    private int pendingResolutionIndex;
    

    void Start()
    {
        LoadResolutions();
        LoadSettings();
    }

    void LoadResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        int currentResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20f);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20f);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20f);
    }


    public void SetVSync(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int index)
    {
        pendingResolutionIndex = index;
    }
    
    public void ApplyResolution()
    {
        Resolution res = resolutions[pendingResolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayClick();
    }



    public void BackToMain()
    {
        PlayClick();
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void PlayClick()
    {
        if (clickSound != null)
            clickSound.Play();
    }

    void LoadSettings()
    {
        vsyncToggle.isOn = QualitySettings.vSyncCount > 0;
        fullscreenToggle.isOn = Screen.fullScreen;
    }
}
