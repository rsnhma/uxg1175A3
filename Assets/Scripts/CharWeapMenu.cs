using UnityEngine;
using UnityEngine.SceneManagement;

public class CharWeapMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(4);
    }
}
