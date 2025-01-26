using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public GameObject menu;
    public GameObject help;
    public float transitionTime = 1f;
    public Animator GetTransition()
    {
        return transition;
    }

    void Update()
    {
        if(transition.GetBool("Start"))
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        else if(transition.GetBool("Help"))
            StartCoroutine(LoadHelp());
        else if(transition.GetBool("Died")) {
            StartCoroutine(LoadLevel(0));
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadHelp()
    {
        yield return new WaitForSeconds(transitionTime);
        menu.SetActive(!menu.activeSelf);
        help.SetActive(!help.activeSelf);

    }
}
