using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public GameObject winScreen;
    private void Awake()
    {
        winScreen = GameObject.FindGameObjectWithTag("win");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        winScreen.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
