using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CharManager : MonoBehaviour
{
    public SpriteRenderer sr; // For sprite preview
    public List<Sprite> characterSprites; // Sprites for preview
    public List<GameObject> characterPrefabs; // Prefabs for actual characters

    private int selectedChar = 0;
    public static int SelectedIndex = 0;

    void Start()
    {
        UpdateCharacterPreview();
    }

    public void NextOption()
    {
        selectedChar++;
        if (selectedChar >= characterSprites.Count)
            selectedChar = 0;

        UpdateCharacterPreview();
    }

    public void PrevOption()
    {
        selectedChar--;
        if (selectedChar < 0)
            selectedChar = characterSprites.Count - 1;

        UpdateCharacterPreview();
    }

    void UpdateCharacterPreview()
    {
        sr.sprite = characterSprites[selectedChar];
    }

    public void PlayGame()
    {
        SelectedIndex = selectedChar;
        SceneManager.LoadScene(4);
    }
}