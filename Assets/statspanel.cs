using UnityEngine;

public class statspanel : MonoBehaviour
{
    public GameObject Panel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ClosePanel()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
    }
}