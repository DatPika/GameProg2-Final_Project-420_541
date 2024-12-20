using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton
    public GameObject PauseMenu;
    public UnityEvent GamePaused;
    public UnityEvent GameResumed;
    private bool isPaused;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called once
    void Start()
    {
         Cursor.visible = false;
    }

    // Called every frame
    void Update()
    {
        ProcessInputs();
    }

    void ProcessInputs()
    {
        if (Input.GetButtonDown("Escape"))
        {
            // To toggle the pause state
            isPaused = !isPaused;
            // Scale time accordingly
            if (isPaused)
            {
                PauseMenu.SetActive(true);
                Time.timeScale = 0f;
                Cursor.visible = true;
                GamePaused.Invoke(); // disables some player scripts and pauses audio
            }
            else
            {
                PauseMenu.SetActive(false);
                Time.timeScale = 1f;
                Cursor.visible = false;
                GameResumed.Invoke(); // enables those same player scripts and resumes audio
            }
        }
    }

    // We want to have this function separate to call it when resume is clicked
    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        GameResumed.Invoke(); // enables those same player scripts and resumes audio
    }

    // Also for pause menu UI for exit button
    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        //Before loading the main menu again, we have to reset time
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
