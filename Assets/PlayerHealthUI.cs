using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerHealthUI : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite noHeart;

    public Image[] hearts; // Assign these via Inspector in order

    private void OnEnable()
    {
        PlayerStats.Instance.onHealthChangedCallback += UpdateHearts;
    }

    /*private void OnDisable()
    {
        PlayerStats.Instance.onHealthChangedCallback -= UpdateHearts;
    }*/

    void Start()
    {
        PlayerStats.Instance.onHealthChangedCallback += UpdateHearts;
        UpdateHearts(); // Initialize
    }

    void UpdateHearts()
    {
        float currentHealth = PlayerStats.Instance.Health;
        float maxHealth = PlayerStats.Instance.MaxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < maxHealth)
            {
                hearts[i].enabled = true;
                if (currentHealth >= i + 1)
                {
                    hearts[i].sprite = fullHeart;
                }
                else if (currentHealth >= i + 0.5f)
                {
                    hearts[i].sprite = halfHeart;
                }
                else
                {
                    hearts[i].sprite = noHeart;
                }
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
