using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Animator transition;
    public LevelLoader levelLoader; 
    public AudioSource audioSource;    

    void Start()
    {
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
