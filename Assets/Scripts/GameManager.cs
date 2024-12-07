using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

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
        if (Input.GetButton("Escape"))
        {
            Cursor.visible = true;
            SceneManager.LoadScene("PauseMenu");
        }
    }

    public void LoadNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
