using UnityEngine;

public class tutorial : MonoBehaviour
{
    public GameObject Panel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OpenPanel()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
    }
}
