using UnityEngine;
using UnityEngine.Audio; 

public class SettingsManager : MonoBehaviour
{
    [Header("Аудио")]
    public AudioMixer audioMixer; 

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
       if (isFullscreen)
        {
        
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
    }

    public void SetSoundsVolume(float volume)
    {
        audioMixer.SetFloat("SoundsVol", Mathf.Log10(volume) * 20);
    }
}