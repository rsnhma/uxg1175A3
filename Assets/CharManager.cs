using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;

public class CharManager : MonoBehaviour
{
    public SpriteRenderer sr;

    public List<Sprite> characters = new List<Sprite>();
    private int selectedchar = 0;

    public GameObject playerchar;

    public void NextOption()
    {
        selectedchar = selectedchar + 1;
        if (selectedchar >= characters.Count)
        {
            selectedchar = 0;
        }
        sr.sprite = characters[selectedchar];
    }
    public void PrevOption()
    {
        selectedchar = selectedchar - 1;
        if (selectedchar < 0)
        {
            selectedchar = characters.Count - 1;
        }
        sr.sprite = characters[selectedchar];
    }
    public void PlayGame()
    {
        Debug.Log("seleted sprite" + selectedchar);
        PrefabUtility.SaveAsPrefabAsset(playerchar, "Assets/Characters/selectedchar.prefab");
        SceneManager.LoadScene(4);
    }
}
