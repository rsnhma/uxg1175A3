using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
   // public GameObject pausemenu;
    public static bool gameispaused = false;
    public GameObject pausemenuUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

    }

    public void PauseGame()
    {
        pausemenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameispaused = true;

    }
    public void ResumeGame()
    {
        pausemenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameispaused = false;

    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
