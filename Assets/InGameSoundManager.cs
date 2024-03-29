using UnityEngine;
using UnityEngine.UI;

public class InGameSoundManager : MonoBehaviour
{
    public static InGameSoundManager instance;
    public Slider inGameSlider;
    public AudioSource clickSound;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        float savedVolume;
        if (PlayerPrefs.HasKey("Sound"))
        {
            savedVolume = PlayerPrefs.GetFloat("Sound");
            inGameSlider.value = savedVolume;
            AudioListener.volume = savedVolume;
        }
        else
        {
            inGameSlider.value = 1f; // Default to maximum volume if no saved value found
        }
    }

    public void SetInGameListenersVolume()
    {
        PlayerPrefs.SetFloat("Sound", inGameSlider.value);
        AudioListener.volume = inGameSlider.value;
    }

    public void PlayClickSound()
    {
        clickSound.Play();
    }
}