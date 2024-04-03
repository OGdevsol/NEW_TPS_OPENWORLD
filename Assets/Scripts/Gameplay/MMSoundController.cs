using UnityEngine;
using UnityEngine.UI;

public class MMSoundController : MonoBehaviour
{
    public static MMSoundController instance;
    public Camera mainMenuListener;
    public Slider mainMenuSlider;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Sound", 1f); // Default to maximum volume if no saved value found
        mainMenuSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
    }

    public void SetMainMenuListenerVolume()
    {
        PlayerPrefs.SetFloat("Sound", mainMenuSlider.value);
        AudioListener.volume = mainMenuSlider.value;
    }
}