using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Animator transition;
    public LevelLoader levelLoader; 
    public AudioSource audioSource;    
    public Slider slider;

    void Start()
    {
        audioSource.volume = MusicVolume.Volume;
        slider.value = MusicVolume.Volume;
        transition = levelLoader.GetTransition();
    }

    public void PlayGame()
    {
        transition.SetTrigger("Start");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HelpToggle()
    {
        transition.SetTrigger("Help");
    }

    public void MusicSliderTogled(float amount) 
    {
        MusicVolume.Volume = amount;
        audioSource.volume = MusicVolume.Volume;
    }
}
