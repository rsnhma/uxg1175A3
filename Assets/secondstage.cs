using UnityEngine;
using UnityEngine.SceneManagement;

public class secondstage : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(6);
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