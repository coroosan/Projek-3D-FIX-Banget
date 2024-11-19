using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgmAudioSource; // AudioSource untuk BGM
    public AudioSource sfxAudioSource; // AudioSource untuk SFX
    public Slider bgmSlider; // Slider untuk BGM
    public Slider sfxSlider; // Slider untuk SFX

    void Start()
    {
        // Set slider sesuai dengan volume saat ini
        bgmSlider.value = PlayerPrefs.GetFloat("BGM_Volume", 1f); // Default 1 (maksimal)
        sfxSlider.value = PlayerPrefs.GetFloat("SFX_Volume", 1f); // Default 1 (maksimal)

        // Update volume saat slider berubah
        bgmSlider.onValueChanged.AddListener(UpdateBGMVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    // Update volume BGM
    public void UpdateBGMVolume(float volume)
    {
        bgmAudioSource.volume = volume;
        PlayerPrefs.SetFloat("BGM_Volume", volume); // Simpan volume BGM
    }

    // Update volume SFX
    public void UpdateSFXVolume(float volume)
    {
        sfxAudioSource.volume = volume;
        PlayerPrefs.SetFloat("SFX_Volume", volume); // Simpan volume SFX
    }
}
