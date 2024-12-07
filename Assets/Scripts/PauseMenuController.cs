using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public void ResumeLevel1()
    {
        SceneManager.LoadScene("Intro-Hike");
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}