using UnityEngine;
using UnityEngine.SceneManagement;

public class skipdialogue : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Skip()
    {
        SceneManager.LoadSceneAsync(6);
    }

}