using UnityEngine;
using UnityEngine.UI;

public class AudioListenerVolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;

    private const string VolumeKey = "Volume";

    private void Start()
    {
        // Set the initial value of the slider to the stored volume value or the default value (0.5f)
        volumeSlider.value = PlayerPrefs.GetFloat(VolumeKey, 1f);
        SetAudioListenerVolume(volumeSlider.value);
    }

    public void OnVolumeSliderValueChanged()
    {
        float volume = volumeSlider.value;
        SetAudioListenerVolume(volume);

        // Store the volume value in PlayerPrefs
        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();
    }

    private void SetAudioListenerVolume(float volume)
    {
        // Update the audio listener volume based on the input value
        AudioListener.volume = volume;
    }
}
